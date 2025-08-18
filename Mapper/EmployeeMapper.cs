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

                CreateMap<AttendenceMarkRequestDto, Attendance>();
                CreateMap<Attendance, AttendenceResponseDto>();
                CreateMap<AttendenceUpdateRequestDto, Attendance>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            }
        }
    }
}
