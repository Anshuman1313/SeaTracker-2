using Assiginment.DTO;
using Assiginment.Models;
using AutoMapper;

namespace Assiginment.Mapper
{
    public class EmployeeMapper
    {
        public class AutoMapperProfile : Profile
        {
            public AutoMapperProfile()
            {
                CreateMap<Employee, EmployeeDto>().ReverseMap();
                CreateMap<Leaf, ApplyLeaveDto>().ReverseMap();
            }
        }
    }
}
