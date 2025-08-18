using Assiginment.DTO;
using Assiginment.Models;

namespace Assiginment.Services
{
    public interface IAttendanceService
    {
        Task<ResponseModel<AttendenceResponseDto>> 
            MarkAttendanceAync(AttendenceMarkRequestDto dto);

        Task<ResponseModel<AttendenceResponseDto>>
            GetAttendanceByEmployeeIdAsync(Guid employeeId, DateOnly date);

        Task<ResponseModel<bool>>
            UpdateAttendanceAsync(int attendanceId, AttendenceUpdateRequestDto dto);

    }
}
