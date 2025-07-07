using System;
using System.Collections.Generic;

namespace DangKyKhoaHoc.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string CourseDescription { get; set; } = null!;

    public string? Instructor { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int Capacity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CourseAnnouncement> CourseAnnouncements { get; set; } = new List<CourseAnnouncement>();

    public virtual ICollection<CourseMaterial> CourseMaterials { get; set; } = new List<CourseMaterial>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
