namespace Assiginment.DTO
{
    public class AttendenceMarkRequestDto
    {
        public Guid EmployeeId { get; set; }
        public DateOnly AttendanceDate { get; set; }        
        public TimeOnly? CheckInTime { get; set; }          
        public TimeOnly? CheckOutTime { get; set; }         
        public string? Remarks { get; set; }                 

    }
}
