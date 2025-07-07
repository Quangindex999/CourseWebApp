using DangKyKhoaHoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;    

namespace DangKyKhoaHoc.Controllers
{
    public class CourseMaterialsController : Controller
    {
        private readonly DangKyKhoaHocContext _context;
        public CourseMaterialsController(DangKyKhoaHocContext context)
        {
            _context = context;
        }

        [HttpGet]
       public IActionResult Upload(int courseId)
        {
            var role = HttpContext.Session.GetString("Role");
            var fullName = HttpContext.Session.GetString("FullName");
            //chỉ cho teacher là instructor hoặc admin upload tài liệu

            var course = _context.Courses
                .FirstOrDefault(c => c.CourseId == courseId);
            if (course == null || (role == "teacher" && course.Instructor != fullName && role != "admin"))
            {
                return RedirectToAction("AccessDenied", "Account");
            }    
            ViewBag.Course = course;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int courseId, IFormFile file)
        {
            var role = HttpContext.Session.GetString("Role");
            var fullName = HttpContext.Session.GetString("FullName");
            var course = _context.Courses
                .FirstOrDefault(c => c.CourseId == courseId);

            if (course == null || (role == "teacher" && course.Instructor != fullName && role != "admin"))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (file != null && file.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "materials");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploads, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var material = new CourseMaterial
                {
                    CourseId = courseId,
                    Title = file.FileName,
                    FilePath = $"/materials/{uniqueFileName}",
                    UploadAt = DateTime.Now,
                };
                _context.CourseMaterials.Add(material);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Tải tài liệu lên thành công";
                return RedirectToAction("List", new {courseId});
            }
            TempData["Message"] = "Vui lòng chọn file để upload.";
            ViewBag.Course = course;
            return View();
        }

      

        public IActionResult List(int courseId)
        {
            var materials = _context.CourseMaterials
                .Include(m => m.Course)
                .Where(m => m.CourseId == courseId)
                .OrderByDescending(m => m.UploadAt)
                .ToList();

            ViewBag.CourseId = courseId;
            return View(materials);
        }

        [HttpPost]
        public IActionResult Delete(int id, int courseId)
        {
            var role = HttpContext.Session.GetString("Role");
            var fullName = HttpContext.Session.GetString("FullName");

            var material = _context.CourseMaterials.Include(m => m.Course)
                .FirstOrDefault(m => m.MaterialsId == id);
            if (material == null)
            {
                TempData["Message"] = "Tài liệu không tồn tại!";
                return RedirectToAction("List", new { courseId });
            }
            //chỉ admin hoặc instructor của khóa học mới có quyền xóa tài liệu
            if (!(role == "admin" || (role == "teacher" && material.Course != null && material.Course.Instructor == fullName)))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            //xoá file vật lý nếu tồn tại
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "materials",
                Path.GetFileName(material.FilePath));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            _context.CourseMaterials.Remove(material);
            _context.SaveChanges();
            TempData["Message"] = "Tài liệu đã được xóa thành công.";
            return RedirectToAction("List", new { courseId });

        }
    }
}
