using Assiginment.Constants;
using Assiginment.DTO;
using Assiginment.Mapper;
using Assiginment.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;

namespace Assiginment.Services
{
    public class EmployeeService :IEmployeeService
    {
        private readonly DevContext _context;
        private IMapper _mapper;

        public EmployeeService(DevContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.IsDeleted == false && e.EmployeeId ==id);

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

                if (await _context.Employees.AnyAsync(e => e.Email == employee.Email))
                {
                    throw new Exception("An employee with the same email already exists.");
                }

                var mappedEmployee = _mapper.Map<Employee>(employee);

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

                return new ResponseModel<EmployeeDto>
                {
                    code = StatusCodes.Status201Created,
                    msg = "Employee created successfully"
                };
            }
            catch(Exception ex)
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
