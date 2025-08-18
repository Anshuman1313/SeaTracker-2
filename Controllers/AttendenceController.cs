using Assiginment.DTO;
using Assiginment.Models;
using Assiginment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assiginment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendenceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ILogger<AttendenceController> _logger;

        public AttendenceController(IAttendanceService attendanceService, ILogger<AttendenceController> logger)
        {
            _attendanceService = attendanceService;
            _logger = logger;
        }
        private IActionResult ApiResponse<T>(ResponseModel<T> result)
        {
            return StatusCode(result.code, new
            {
                success = result.code < 400,
                message = result.msg,
                data = result.data
            });
        }
        [HttpPost("mark")] 
        public async Task<IActionResult> MarkAttendance([FromBody] AttendenceMarkRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Marking attendance for EmployeeId: {EmployeeId}", dto.EmployeeId);
                var result = await _attendanceService.MarkAttendanceAync(dto);
                return ApiResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking attendance");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetAttendanceByEmployee(Guid employeeId, [FromQuery] DateOnly date)
        {
            if (employeeId == Guid.Empty)
                return BadRequest("Invalid Employee ID");

            try
            {
                _logger.LogInformation("Fetching attendance for EmployeeId: {EmployeeId} on {Date}", employeeId, date);
                var result = await _attendanceService.GetAttendanceByEmployeeIdAsync(employeeId, date);
                return ApiResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching attendance");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        [HttpPatch("{attendanceId}")]
        public async Task<IActionResult> UpdateAttendance(int attendanceId, [FromBody] AttendenceUpdateRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Updating attendance ID: {AttendanceId}", attendanceId);
                var result = await _attendanceService.UpdateAttendanceAsync(attendanceId, dto);
                return ApiResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating attendance");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

    }
}
