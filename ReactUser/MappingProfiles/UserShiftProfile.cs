using AutoMapper;
using ReactUser.Models.Dto;
using ReactUser.Models;
using Talento_API.Models;
using Talento_API.Models.Dto;

namespace Talento_API.MappingProfiles
{
    public class UserShiftProfile: Profile
    {
        public UserShiftProfile()
        {
            CreateMap<UserShiftCreateDTO, UserShift>();
            CreateMap<UserShiftUpdateDTO, UserShift>();

            CreateMap<UserShift, UserShiftUpdateDTO>();
            CreateMap<UserShift, UserShiftDTO>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
               .ForMember(dest => dest.ShiftName, opt => opt.MapFrom(src => src.Shift.ShiftName));

        }
    }
}
