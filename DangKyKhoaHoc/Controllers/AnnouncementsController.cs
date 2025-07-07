using DangKyKhoaHoc.Models;
using Microsoft.AspNetCore.Mvc;

namespace DangKyKhoaHoc.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly DangKyKhoaHocContext _context;
        public AnnouncementsController(DangKyKhoaHocContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if(role != "admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }    
            var announcements = _context.CourseAnnouncements
                .OrderByDescending(a => a.CreatedAt)
                .ToList();
            return View(announcements);
        }
    }
}
