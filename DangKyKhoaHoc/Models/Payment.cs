﻿using System;
using System.Collections.Generic;

namespace DangKyKhoaHoc.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? UserId { get; set; }

    public int? CourseId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public virtual Course? Course { get; set; }

    public virtual User? User { get; set; }
}
