using System;
using System.Collections.Generic;

namespace DangKyKhoaHoc.Models;

public partial class CourseMaterial
{
    public int MaterialsId { get; set; }

    public int? CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string FilePath { get; set; } = null!;
  
    public DateTime? UploadAt { get; set; }

    public virtual Course? Course { get; set; }
}
