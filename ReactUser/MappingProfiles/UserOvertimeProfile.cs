using AutoMapper;
using ReactUser.Models.Dto;
using ReactUser.Models;
using Talento_API.Models.Dto;
using Talento_API.Models;

namespace Talento_API.MappingProfiles
{
    public class UserOvertimeProfile: Profile
    {
        public UserOvertimeProfile()
        {
            CreateMap<UserOvertime, UserOvertimeDTO>();
            CreateMap<UserOvertimeCreateDTO, UserOvertime>();
            CreateMap<UserOvertimeUpdateDTO, UserOvertime>();

            CreateMap<UserOvertime, UserOvertimeUpdateDTO>();
        }

    }
}
