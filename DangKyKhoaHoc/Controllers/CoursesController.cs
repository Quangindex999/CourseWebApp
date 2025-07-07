using DangKyKhoaHoc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Model.Structures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangKyKhoaHoc.Controllers
{
    public class CoursesController : Controller
    {
        private readonly DangKyKhoaHocContext _context;

        public CoursesController(DangKyKhoaHocContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchTerm)
        {
            
            var courses = _context.Courses.AsQueryable();
            //Bắt đầu với tất cả các khoá học, sau đó có thể lọc nếu có từ khoá.

            //kiểm tra nếu có từ khoá tìm kiếm
            if(!string.IsNullOrEmpty(searchTerm))
            {
                //lọc khoá học theo tiêu đề hoặc mô tả
                courses = courses.Where(c => c.Title.Contains(searchTerm) || c.CourseDescription.Contains(searchTerm));
            }

            //trả về danh sách khoá học đã lọc hoặc tất cả nếu không có từ khoá
            return View(await courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            var feedbacks = _context.FeedBacks
                .Where(f => f.CourseId == id)
                .ToList();
            var materials = _context.CourseMaterials
                .Where(m => m.CourseId == id)
                .OrderByDescending(m => m.UploadAt)
                .ToList();
            var model = new CourseDetailVM
            {
                Course = course,
                Feedbacks = feedbacks,
                Materials = materials
            };

            return View(model);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Title,CourseDescription,Instructor,StartDate,EndDate,Capacity,CreatedAt")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            //kiểm tra quyền
            var role = HttpContext.Session.GetString("Role");
            var fullName = HttpContext.Session.GetString("FullName");
            if (role == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (role == "admin" || (role == "teacher" & course.Instructor == fullName))
            {
                return View(course); // đủ quyền, cho sửa
            }

            else
            {
                return RedirectToAction("AccessDenied", "Account"); //không đủ quyền , chuyển hướng đến trang từ chối truy cập
            }    

        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Title,CourseDescription,Instructor,StartDate,EndDate,Capacity,CreatedAt")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            var role = HttpContext.Session.GetString("Role");
            var fullName = HttpContext.Session.GetString("FullName");

            if (role == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (role == "admin" || (role == "teacher" && course.Instructor == fullName))
            {
                return View(course); // đủ quyền, cho xóa
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account"); //không đủ quyền , chuyển hướng đến trang từ chối truy cập    
            }
        }
        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            var role = HttpContext.Session.GetString("Role");
            var fullName = HttpContext.Session.GetString("FullName");

            if (role == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (role == "admin" || (role == "teacher" && course.Instructor == fullName))
            {

                _context.Courses.Remove(course); // đủ quyền, cho xóa
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }
            else
            {
                return RedirectToAction("AccessDenied", "Account"); //không đủ quyền , chuyển hướng đến trang từ chối truy cập    
            }
        }
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
        [HttpPost]
        public IActionResult Enroll(int courseId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //kiểm tra xem người dùng đã đăng ký khóa học này chưa
            var existingEnrollment = _context.Enrollments
                .FirstOrDefault(e => e.UserId == userId && e.CourseId == courseId);
            if (existingEnrollment != null)
            {
                TempData["Message"] = "Bạn đã đăng ký khóa học này rồi.";
                return RedirectToAction("Index");
            }
            var enrollment = new Enrollment
            {
                UserId = userId,
                CourseId = courseId,
                EnrollAt = DateTime.Now
            };
            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();

            TempData["Message"] = "Đăng ký khóa học thành công.";
            return RedirectToAction("Index");
        }

        public IActionResult MyCourses()
        {
            //lấy userId từ session
            var userId = HttpContext.Session.GetInt32("UserID");

            //Nếu không có userId thì chuyển hướng đến trang đăng nhập
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //Truy vấn danh sách khoá học đã đăng ký của người dùng
            var enrolledCourses = _context.Enrollments
                .Where(e => e.UserId == userId) //lọc theo userId
                .Select(e => e.Course) //lấy thông tin khoá học
                .ToList();              //chuyển thành danh sách

            //trả về view với danh sách khoá học đã đăng ký
            return View(enrolledCourses);
        }
        [HttpPost]
        public IActionResult Unenroll(int courseId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var enrollment = _context.Enrollments
                .FirstOrDefault(e => e.UserId == userId && e.CourseId == courseId);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                _context.SaveChanges();
                TempData["Message"] = "Hủy đăng ký khóa học thành công.";
            }
            else
            {
                TempData["Message"] = "Không tìm thấy thông tin đăng ký";
            }
            return RedirectToAction("MyCourses");
        }
        //Action để hiển thị form đánh giá khóa học
        [HttpGet]
        public IActionResult FeedBack(int courseId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //Kiểm tra xem đã đánh giá chưa
            var existedFeedback = _context.FeedBacks
                .FirstOrDefault(f => f.UserId == userId && f.CourseId == courseId);
            if (existedFeedback != null)
            {
                TempData["Message"] = "Bạn đã đánh giá khóa học này rồi.";
                return RedirectToAction("MyCourses");
            }
            //truyền thông tin khoá học vào view
            var course = _context.Courses.Find(courseId);
            ViewBag.Course = course;
            return View();
        }

        //xử lý lưu đánh giá
        [HttpPost]
        public IActionResult FeedBack(int courseId, int rating, string comment)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //Kiểm tra xem đã đánh giá chưa
            var existedFeedback = _context.FeedBacks
                .FirstOrDefault(f => f.UserId == userId && f.CourseId == courseId);

            if (existedFeedback != null)
            {
                TempData["Message"] = "Bạn đã đánh giá khoá học này rồi";
                return RedirectToAction("MyCourses");
            }

            //Nếu chưa đánh giá thì trả về view để người dùng nhập đánh giá
            var feedback = new FeedBack
            {
                UserId = userId.Value,
                CourseId = courseId,
                Rating = rating,
                Comment = comment,
                SubmittedAt = DateTime.Now
            };
            _context.FeedBacks.Add(feedback);
            _context.SaveChanges();

            TempData["Message"] = "Đánh giá khóa học thành công.";
            return RedirectToAction("MyCourses");
        }

        [HttpGet]
        public IActionResult Manage()
        {
            var role = HttpContext.Session.GetString("Role");
            var fullName = HttpContext.Session.GetString("FullName");
            if (role == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<Course> course;
            if (role == "admin")
            {
                course = _context.Courses.ToList(); // Lấy tất cả khóa học nếu là admin
            }
            else if (role == "teacher")
            {
                course = _context.Courses
                    .Where(c => c.Instructor == fullName)
                    .ToList(); // Lấy khóa học của giảng viên nếu là teacher
            }
            else
            {
                return RedirectToAction("AccessDenied", "Courses"); // Nếu không phải admin hoặc teacher, chuyển hướng về trang khóa học
            }
            return View(course);
        }

        public IActionResult MyTeachingCourses()
        {
            var role = HttpContext.Session.GetString("Role");
            var fullName = HttpContext.Session.GetString("FullName");

            if(role != "teacher")
            {
                return RedirectToAction("AccessDenied", "Account"); // Nếu không phải teacher, chuyển hướng về trang từ chối truy cập
            }
            //lấy danh sách khoá học mà Instructor là giảng viên hiện tại
            var myCourses = _context.Courses
                .Where(c => c.Instructor == fullName)
                .ToList();
            return View(myCourses);
        }

        [HttpGet]
        public IActionResult CreateAnnouncement(int courseId)
        {
            var role = HttpContext.Session.GetString("Role");
            var fullName = HttpContext.Session.GetString("FullName");

            // Kiểm tra quyền admin hoặc teacher là Instructor của khoá học
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);
            if (course == null || (role != "admin" && course.Instructor != fullName))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            ViewBag.CourseId = courseId;
            return View();
        }

        //action lưu thông báo mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  CreateAnnouncement(int courseId, string title, string announcementMessage)
        {
            var role = HttpContext.Session.GetString("Role");
            var fullName = HttpContext.Session.GetString("FullName");

            var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);
            if (course == null || (role != "admin" && course.Instructor != fullName))
            {
                return RedirectToAction("AccessDenied", "Account"); // Kiểm tra quyền
            }

            var announcement = new CourseAnnouncement
            {
                CourseId = courseId,
                Title = title,
                AnnouncementMessage = announcementMessage,
                CreatedAt = DateTime.Now,
            };

            _context.CourseAnnouncements.Add(announcement);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Thông báo đã được đăng thành công";
            return RedirectToAction("MyTeachingCourses", "Courses"); // Chuyển hướng về khoá học tôi giảng dạy
        }

        //hiển thị danh sách thông báo của khóa học
        public IActionResult ViewAnnouncements(int courseId)
        {
            var role = HttpContext.Session.GetString("Role");
            var fullName = HttpContext.Session.GetString("FullName");

            var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);
            if (course == null || (role == "teacher" && course.Instructor != fullName && role != "admin"))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var announcements = _context.CourseAnnouncements
                .Where(a => a.CourseId == courseId)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            ViewBag.CourseId = courseId;
            return View(announcements);
        }
    }

}
