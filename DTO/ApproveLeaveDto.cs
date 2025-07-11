namespace Assiginment.DTO
{
    public class ApproveLeaveDto
    {
        public int LeaveId { get; set; }
        public int ApprovedByUserId { get; set; }

        public string Status { get; set; } = null!;
    }
}
