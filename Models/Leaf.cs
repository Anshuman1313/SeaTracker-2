using System;
using System.Collections.Generic;

namespace Assiginment.Models;

public partial class Leaf
{
    public int LeaveId { get; set; }

    public Guid? EmployeeId { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string LeaveType { get; set; } = null!;

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public DateTime? AppliedAt { get; set; }

    public int? ApprovedBy { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual Employee? Employee { get; set; }
}
