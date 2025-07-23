using System;
using System.Collections.Generic;

namespace Assiginment.Models;

public partial class Employee
{
    public Guid EmployeeId { get; set; }

    public int? UserId { get; set; }
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Department { get; set; }

    public string? Designation { get; set; }

    public DateOnly? JoiningDate { get; set; }

    public bool? IsDeleted { get; set; }
    public  required string ExternalId { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Leaf> Leaves { get; set; } = new List<Leaf>();
    public virtual Image? Image { get; set; }
    public virtual User? User { get; set; }
}
