using AutoMapper;
using ReactUser.Models.Dto;
using ReactUser.Models;

namespace Talento_API.MappingProfiles
{
    public class ApplicationUserProfile: Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, UserListDTO>();
            CreateMap<ApplicationUserCreateDTO, ApplicationUser>();
            CreateMap<ApplicationUserUpdateDTO, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserUpdateDTO>();

        }
    }
}
