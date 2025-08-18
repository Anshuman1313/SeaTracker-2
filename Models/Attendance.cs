
using System;
using System.Collections.Generic;


namespace Assiginment.Models;
public enum AttendanceStatus
{
    Absent,
    Present,
    Leave,
    HalfDay
}

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public Guid? EmployeeId { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public TimeOnly? CheckInTime { get; set; }

    public TimeOnly? CheckOutTime { get; set; }

    public AttendanceStatus Status { get; set; } = AttendanceStatus.Absent;

    public string? Remarks { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public virtual Employee? Employee { get; set; }
}
