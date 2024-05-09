using AutoMapper;
using ReactUser.Models.Dto;
using ReactUser.Models;
using Talento_API.Models;
using Talento_API.Models.Dto;

namespace Talento_API.MappingProfiles
{
    public class WorkingDayProfile: Profile
    {
        public WorkingDayProfile()
        {
            CreateMap<WorkingDayCreateDTO, WorkingDay>();
            CreateMap<WorkingDayUpdateDTO, WorkingDay>();
            CreateMap<WorkingDay, WorkingDayDTO>();
            CreateMap<WorkingDay, WorkingDayUpdateDTO>();
        }
    }
}
