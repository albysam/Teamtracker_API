using AutoMapper;
using ReactUser.Models.Dto;
using ReactUser.Models;

namespace Talento_API.MappingProfiles
{
    public class DepartmentProfile: Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentCreateDTO, Department>();
            CreateMap<DepartmentUpdateDTO, Department>();
            CreateMap<Department, DepartmentDTO>();
            CreateMap<Department, DepartmentUpdateDTO>();

        }
    }
}
