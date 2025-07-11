using Assiginment.DTO;
using Assiginment.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assiginment.Services
{
    public class LeaveService  :ILeaveService
    {
        private readonly DevContext _context;
        private IMapper _mapper;
        public LeaveService(DevContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseModel<ApplyLeaveDto>> ApplyLeave(ApplyLeaveDto applyLeave)
        {
            try
            {

                if (applyLeave.FromDate.HasValue && applyLeave.FromDate.Value < DateOnly.FromDateTime(DateTime.Today))
                {
                    throw new Exception("From date is not valid as it is in past");
                }

                var mappedEmployee_Leave = _mapper.Map<Leaf>(applyLeave);
                mappedEmployee_Leave.Status = "Pending";
                mappedEmployee_Leave.AppliedAt = DateTime.Now;

                _context.Leaves.Add(mappedEmployee_Leave);
                await _context.SaveChangesAsync();

                return new ResponseModel<ApplyLeaveDto>
                {
                    code = StatusCodes.Status201Created,
                    msg = "Applied for leave successfully"
                    
                };
            }
            catch 
            {
                throw new Exception("THe error is in service of leave");
            }

        }
    }
}
