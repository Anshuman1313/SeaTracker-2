using System;
using Assiginment.Constants;
using Assiginment.DTO;
using Assiginment.Mapper;
using Assiginment.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assiginment.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DevContext _context;
        private IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ICommonService _commonService;


        public EmployeeService(DevContext context, IMapper mapper, IEmailService emailService,ICommonService commonService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _commonService = commonService;
        }

        public async Task<ListResponseModel<EmployeeDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var allEmployees = await _context.Employees
                .OrderByDescending(e => e.EmployeeId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Where(e => e.IsDeleted == false)
                .Select(x => new EmployeeDto
                {
                    EmployeeId = x.EmployeeId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Phone = x.Phone,
                    Department = x.Department,
                    Designation = x.Designation,
                    JoiningDate = x.JoiningDate,

                })
                .ToListAsync();

            var totalItems = await _context.Employees.CountAsync();


            return new ListResponseModel<EmployeeDto>
            {
                data = allEmployees,
                totalItems = totalItems,
                code = "200",
                msg = "Employees retrieved successfully"
            };
        }

        public async Task<ResponseModel<Employee>> GetByIdAsync(Guid id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.IsDeleted == false && e.EmployeeId == id);

            return new ResponseModel<Employee>
            {
                data = employee,
                code = StatusCodes.Status200OK,
                msg = employee != null ? "Employee retrieved successfully" : "Employee not found"
            };
        }

        public async Task<ResponseModel<EmployeeDto>> CreateAsync(EmployeeDto employee)
        {
            try
            {
                //anyasync runs a sql query under the hood that searches in employee db and see for email
                if (await _context.Employees.AnyAsync(e => e.Email == employee.Email))
                {
                    throw new Exception("An employee with the same email already exists.");
                }
                var tempPassword = _commonService.GenerateTempPassword();

                var newUser = new User
                {
                    UserName = employee.Email,
                    Role = "Employee",
                    //PasswordHash = tempPassword,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword),
                    isFirstLogin = true,
                    IsActive = true, // this for the soft delete functionlaity
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var mappedEmployee = _mapper.Map<Employee>(employee);
                mappedEmployee.UserId = newUser.UserId;

                //if image is sent
                if (!string.IsNullOrEmpty(employee.ImageBase64))
                {
                    mappedEmployee.Image = new Image
                    {
                        ImageBase64 = employee.ImageBase64
                    };
                }


                _context.Employees.Add(mappedEmployee);
                await _context.SaveChangesAsync();
                
                //  Send email with credentials
                var subject = "Welcome to Sea Tracker – Your Account is Ready";

                    var body = $@"
                Hello {employee.FirstName},<br/><br/>
                Your account has been created on Sea Tracker.<br/>
                <b>Login Email:</b> {employee.Email}<br/>
                <b>Temporary Password:</b> {tempPassword}<br/><br/>
                Please log in and change your password after your first login.";

                await _emailService.FirstPasswordEmailAsync(employee.Email, subject, body);


                return new ResponseModel<EmployeeDto>
                {
                    code = StatusCodes.Status201Created,
                    msg = "Employee created successfully"
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> UpdateAsync(Guid id, EmployeeDto updatedEmployee)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
                return "NotFound";


            // Check if email is being changed and if the new email already exists for another employee
            if (!string.IsNullOrEmpty(updatedEmployee.Email) &&
                existingEmployee.Email != updatedEmployee.Email &&
                await _context.Employees.AnyAsync(e => e.Email == updatedEmployee.Email && e.EmployeeId != id))
            {
                return "EmailExists";
            }

            // Update relevant fields (excluding navigation properties and sensitive fields)
            existingEmployee.FirstName = updatedEmployee.FirstName;
            existingEmployee.LastName = updatedEmployee.LastName;
            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.Phone = updatedEmployee.Phone;
            existingEmployee.Department = updatedEmployee.Department;
            existingEmployee.Designation = updatedEmployee.Designation;
            existingEmployee.JoiningDate = updatedEmployee.JoiningDate;
            //existingEmployee.IsDeleted = updatedEmployee.IsDeleted;

            // Optionally: don't allow updating UserId from here unless explicitly needed
            // existingEmployee.UserId = updatedEmployee.UserId;

            await _context.SaveChangesAsync();
            return "Success";
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return false;


            employee.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
