using Assiginment.DTO;
using Assiginment.Models;


namespace Assiginment.Services
{
    public interface IEmployeeService
    {
        Task<ListResponseModel<EmployeeDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<ResponseModel<Employee>> GetByIdAsync(Guid id);
        Task<ResponseModel<EmployeeDto>> CreateAsync(EmployeeDto employee);
        Task<string> UpdateAsync(Guid id, EmployeeDto employee);
        Task<bool> DeleteAsync(Guid id);
    }
}
