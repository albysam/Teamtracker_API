using AutoMapper;
using ReactUser.Models.Dto;
using ReactUser.Models;

namespace Talento_API.MappingProfiles
{
    public class UserLeaveProfile: Profile
    {
        public UserLeaveProfile()
        {
            CreateMap<UserLeaveCreateDTO, UserLeave>();
            CreateMap<UserLeaveUpdateDTO, UserLeave>();

            CreateMap<UserLeave, UserLeaveUpdateDTO>();
            CreateMap<UserLeave, UserLeaveDTO>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
               .ForMember(dest => dest.LeaveName, opt => opt.MapFrom(src => src.Leave.LeaveName));

        }
    }
}
