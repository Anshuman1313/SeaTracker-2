using Assiginment.DTO;
using Assiginment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assiginment.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {   
        private readonly ILeaveService _leaveService;
        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }
        [HttpPost]
        public async Task<IActionResult> ApplyLeave([FromBody] ApplyLeaveDto applyLeave)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var created = await _leaveService.ApplyLeave(applyLeave);
            return  Ok(new
            {
                data = created,
                msg = "Leave applied Successfully",
                code = "201"
            });
        }


    }
}
