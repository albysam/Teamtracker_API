using AutoMapper;
using ReactUser.Models.Dto;
using ReactUser.Models;
using Talento_API.Models.Dto;
using Talento_API.Models;

namespace Talento_API.MappingProfiles
{
    public class UserOndutyProfile: Profile
    {
        public UserOndutyProfile()
        {
            CreateMap<UserOnduty, UserOndutyDTO>();
            CreateMap<UserOndutyCreateDTO, UserOnduty>();
            CreateMap<UserOndutyUpdateDTO, UserOnduty>();

            CreateMap<UserOnduty, UserOndutyUpdateDTO>();
        }

    }
}
