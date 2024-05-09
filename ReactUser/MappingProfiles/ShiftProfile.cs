using AutoMapper;
using ReactUser.Models.Dto;
using ReactUser.Models;
using Talento_API.Models;
using Talento_API.Models.Dto;

namespace Talento_API.MappingProfiles
{
    public class ShiftProfile: Profile
    {
        public ShiftProfile()
        {
            CreateMap<ShiftCreateDTO, Shift>()
.ForMember(dest => dest.TotalShiftDuration, opt => opt.MapFrom(src => int.Parse(src.ShiftDuration) + int.Parse(src.BreakDuration)));

            CreateMap<ShiftUpdateDTO, Shift>()
                .ForMember(dest => dest.TotalShiftDuration, opt => opt.MapFrom(src => int.Parse(src.ShiftDuration) + int.Parse(src.BreakDuration)));

            CreateMap<Shift, ShiftUpdateDTO>()
                .ForMember(dest => dest.TotalShiftDuration, opt => opt.MapFrom(src => src.ShiftDuration.ToString() + src.BreakDuration.ToString()));
        }
    }
}
