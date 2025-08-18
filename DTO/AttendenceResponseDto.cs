namespace Assiginment.DTO
{
    public class AttendenceResponseDto
    {
        public int AttendanceId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateOnly AttendanceDate { get; set; }
        public TimeOnly? CheckInTime { get; set; }
        public TimeOnly? CheckOutTime { get; set; }
        public string Status { get; set; }       // Return as string for client readability
        public string? Remarks { get; set; }
    }
}
//The Response DTO is what you send back to the client after processing attendance.

//Its role:
//Confirm what the system recorded.
//Include calculated or system-set values like Status (Present / Absent / Leave).

