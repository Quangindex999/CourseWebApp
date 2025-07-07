using System;
using System.Collections.Generic;

namespace DangKyKhoaHoc.Models;

public partial class FeedBack
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }

    public int? CourseId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public virtual Course? Course { get; set; }

    public virtual User? User { get; set; }
}
