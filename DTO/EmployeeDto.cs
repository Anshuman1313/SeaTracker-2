namespace Assiginment.DTO
{
    public class EmployeeDto
    {
        public Guid EmployeeId { get; set; }
        public int? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public string? ExternalId { get; set; } 
        public DateOnly? JoiningDate { get; set; }
    }
}
