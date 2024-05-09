using AutoMapper;
using ReactUser.Models.Dto;
using ReactUser.Models;

namespace Talento_API.MappingProfiles
{
    public class DesgnationProfile: Profile
    {
        public DesgnationProfile()
        {
            CreateMap<Desgnation, DesgnationDTO>();
            CreateMap<DesgnationCreateDTO, Desgnation>();
            CreateMap<DesgnationUpdateDTO, Desgnation>();
            CreateMap<Desgnation, DesgnationUpdateDTO>();

        }
    }
}
