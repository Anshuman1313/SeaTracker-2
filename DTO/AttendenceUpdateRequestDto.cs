using Assiginment.Models;

namespace Assiginment.DTO
{
    public class AttendenceUpdateRequestDto
    {
        public TimeOnly? CheckInTime { get; set; }    
        public TimeOnly? CheckOutTime { get; set; }   
        public string? Remarks { get; set; }

        public string? Status { get; set; }
    }
}
