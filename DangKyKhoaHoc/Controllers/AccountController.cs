using DangKyKhoaHoc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DangKyKhoaHoc.Controllers
{
    public class AccountController : Controller
    {
        private readonly DangKyKhoaHocContext _context;

        public AccountController(DangKyKhoaHocContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                // kiểm tra thông tin đăng nhập 
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == model.Password);

                //Lưu lịch sử đăng nhập
                var loginHistory = new LoginHistory
                {
                    UserId = user.UserId,
                    LoginTime = DateTime.Now,
                    Ipaddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
                };

                //Lưu vào bảng ghi lịch sử đăng nhập
                _context.LoginHistories.Add(loginHistory);
                _context.SaveChanges();

                if (user != null)
                {
                    // Lưu thông tin user vào session
                    HttpContext.Session.SetInt32("UserID", user.UserId);
                    HttpContext.Session.SetString("FullName", user.FullName ?? "");
                    HttpContext.Session.SetString("Role", user.Role);

                    return RedirectToAction("Index", "Courses");
                }

                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu.");

                
            }
            return View(model);
        }
            // đăng xuất tài khoản
            public IActionResult Logout()
            {
            //xoá toàn bộ session của user
            HttpContext.Session.Clear();
            //chuyển về trang đăng nhập
            return RedirectToAction("Login", "Account");
            }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Register model)
        {
            if(ModelState.IsValid)
            {
                //Kiểm tra xem email đã tồn tại chưa
                if(_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                    return View(model);
                }
                //thêm user mới vào database
                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PasswordHash = model.Password, // Mã hoá mật khẩu nếu cần
                    Role = model.Role,
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(user);
                _context.SaveChanges();// lưu thay đổi vào database

                // đăng nhập ngay sau khi đăng ký
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult LoginHistory()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            var history = _context.LoginHistories
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.LoginTime)
                .ToList();
            return View(history);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        

    }
}
