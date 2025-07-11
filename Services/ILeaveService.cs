using Assiginment.DTO;
using Assiginment.Models;

namespace Assiginment.Services
{
    public interface ILeaveService
    {
        Task<ResponseModel<ApplyLeaveDto>> ApplyLeave(ApplyLeaveDto applyLeave);
    }
}
