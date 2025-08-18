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
                // Skip mapping nulls for all properties  
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
                 CreateMap<AttendenceUpdateRequestDto, Attendance>()
                // Special rule for Status: only map if it's a valid enum  
                .ForMember(dest => dest.Status, opt =>
                {
                    opt.PreCondition(src => Enum.TryParse<AttendanceStatus>(src.Status, true, out _));
                    opt.MapFrom(src => Enum.Parse<AttendanceStatus>(src.Status!, true));
                });

            }
        }
    }
}
