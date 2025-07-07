using System;
using System.Collections.Generic;

namespace DangKyKhoaHoc.Models;

public partial class CourseAnnouncement
{
    public int AnnoucementId { get; set; }

    public int? CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string AnnouncementMessage { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Course? Course { get; set; }
}
