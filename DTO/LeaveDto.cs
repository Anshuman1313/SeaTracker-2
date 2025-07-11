namespace Assiginment.DTO
{
    public class ApplyLeaveDto
    {
        //public int LeaveId { get; set; }

        public Guid? EmployeeId { get; set; }

        public DateOnly? FromDate { get; set; }

        public DateOnly? ToDate { get; set; }

        public string LeaveType { get; set; } = null!;

        public string? Reason { get; set; }

        //public string? Status { get; set; }

        //public DateTime? AppliedAt { get; set; }
    }
}
