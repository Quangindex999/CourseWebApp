using System;
using System.Collections.Generic;

namespace DangKyKhoaHoc.Models;

public partial class LoginHistory
{
    public int HistoryId { get; set; }

    public int? UserId { get; set; }

    public DateTime? LoginTime { get; set; }

    public string? Ipaddress { get; set; }

    public virtual User? User { get; set; }
}
