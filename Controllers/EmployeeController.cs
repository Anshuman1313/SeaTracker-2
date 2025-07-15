using Assiginment.DTO;
using Assiginment.Models;
using Assiginment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assiginment.Controllers
{
      [Authorize]
    //[AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 25)
        {
            var result = await _employeeService.GetAllAsync(pageNumber, pageSize);
            if (result.data == null || !result.data.Any())
            {
                return Ok(new
                {
                    data = new List<Employee>(),
                    totalItems = 0,
                    msg = "No employees found.",
                    code = "200"
                });
            }

            return Ok(new
            {
                data = result.data,
                totalItems = result.totalItems,
                msg = "Employees retrieved successfully",
                code = "200"
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _employeeService.GetByIdAsync(id);
            return result != null
                ? Ok(new { data = result, msg = "Employee found", code = "200" })
                : NotFound(new { Message = $"Employee with ID {id} not found." });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeDto employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _employeeService.CreateAsync(employee);

            if(created.code == StatusCodes.Status201Created)
            {
                return Ok( new
                {
                    msg = "Employee created successfully",
                    code = "201"
                });
            }
          else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EmployeeDto employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _employeeService.UpdateAsync(id, employee);
            return result switch
            {
                "NotFound" => NotFound(new { Message = $"Employee with ID {id} not found." }),
                "EmailExists" => BadRequest(new { Message = "An employee with this email already exists." }),
                "Success" => NoContent(),
                _ => BadRequest("Unable to update the employee.")
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _employeeService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound(new { Message = $"Employee with ID {id} not found." });
        }
    }
}
