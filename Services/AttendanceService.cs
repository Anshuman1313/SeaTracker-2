using AutoMapper;
using Assiginment.DTO;
using Assiginment.Models;
using Microsoft.EntityFrameworkCore;


namespace Assiginment.Services
{
    public class AttendanceService :IAttendanceService
    {
        private readonly DevContext _context;
        private IMapper _mapper;
        private readonly ILogger<AttendanceService> _logger;
        public AttendanceService(DevContext context, IMapper mapper, ILogger<AttendanceService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ResponseModel<AttendenceResponseDto>> MarkAttendanceAync(AttendenceMarkRequestDto dto)
        {
            try
            {   
                var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == dto.EmployeeId);
                if(!employeeExists)
                {
                    return new ResponseModel<AttendenceResponseDto>
                    {
                        code = StatusCodes.Status404NotFound,
                        msg = "Employee not found",
                        data = null
                    };
                }

                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var existingAttendance = await _context.Attendances
                    .AnyAsync(a => a.EmployeeId == dto.EmployeeId && a.AttendanceDate == today);
                if (existingAttendance)
                {
                    return new ResponseModel<AttendenceResponseDto>
                    {
                        code = StatusCodes.Status409Conflict,
                        msg = "Attendance for today already marked",
                        data = null
                    };
                } 
                
                var attendance = _mapper.Map<Attendance>(dto);

                attendance.AttendanceDate = today;
                attendance.Status = AttendanceStatus.Present;
                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();
                var responseDto = _mapper.Map<AttendenceResponseDto>(attendance);
                return new ResponseModel<AttendenceResponseDto>
                {
                    code = StatusCodes.Status201Created,
                    msg = "Attendance marked successfully",
                    data = responseDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while marking attendance for EmployeeId: {EmployeeId}", dto.EmployeeId);
                return new ResponseModel<AttendenceResponseDto>
                {
                    code = StatusCodes.Status500InternalServerError,
                    msg = "An error occurred while marking attendance.",
                    data = null
                };
            }
        }
        public async Task<ResponseModel<AttendenceResponseDto>> GetAttendanceByEmployeeIdAsync(Guid employeeId, DateOnly date)
        {
            try
            {
                var attendance = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.AttendanceDate == date);
                    
                if (attendance == null)
                {
                    return new ResponseModel<AttendenceResponseDto>
                    {
                        code = StatusCodes.Status404NotFound,
                        msg = "Attendance not found",
                        data = null
                    };
                }
                var responseDto = _mapper.Map<AttendenceResponseDto>(attendance);
                return new ResponseModel<AttendenceResponseDto>
                {
                    code = StatusCodes.Status200OK,
                    msg = "Attendance retrieved successfully",
                    data = responseDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error retrieving attendance for EmployeeId: {EmployeeId} on Date: {Date}",employeeId, date);
                return new ResponseModel<AttendenceResponseDto>
                {
                    code = StatusCodes.Status500InternalServerError,
                    msg = "An error occurred while retrieving attendance.",
                    data = null
                };
            }
        }
        public async Task<ResponseModel<bool>> UpdateAttendanceAsync(int attendanceId, AttendenceUpdateRequestDto dto)
        {
            try
            {
                var attendance = await _context.Attendances.FindAsync(attendanceId);
                if (attendance == null)
                {
                    return new ResponseModel<bool>
                    {
                        code = StatusCodes.Status404NotFound,
                        msg = "Attendance not found",
                        data = false
                    };
                }
                _mapper.Map(dto, attendance);
                _context.Attendances.Update(attendance);
                await _context.SaveChangesAsync();
                return new ResponseModel<bool>
                {
                    code = StatusCodes.Status200OK,
                    msg = "Attendance updated successfully",
                    data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error updating attendance with ID: {AttendanceId}",attendanceId);

                return new ResponseModel<bool>
                {
                    code = StatusCodes.Status500InternalServerError,
                    msg = "An error occurred while updating attendance.",
                    data = false
                };
            }
        }

    }
}
