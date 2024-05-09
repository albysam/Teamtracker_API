using AutoMapper;
using ReactUser.Models.Dto;
using ReactUser.Models;
using Talento_API.Models;
using Talento_API.Models.Dto;

namespace Talento_API.MappingProfiles
{
    public class HolidayProfile: Profile
    {
        public HolidayProfile()
        {
            CreateMap<HolidayCreateDTO, Holiday>();
            CreateMap<HolidayUpdateDTO, Holiday>();
            CreateMap<Holiday, HolidayDTO>();
            CreateMap<Holiday, HolidayUpdateDTO>();
        }
    }
}
