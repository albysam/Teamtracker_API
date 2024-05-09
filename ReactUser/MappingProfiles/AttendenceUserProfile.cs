using AutoMapper;
using ReactUser.Models.Dto;
using ReactUser.Models;

namespace Talento_API.MappingProfiles
{
    public class AttendenceUserProfile : Profile
    {
        public AttendenceUserProfile()
        {
            CreateMap<AttendenceUpdateDTO, AttendenceUser>()
 .ForMember(dest => dest.WorkedHours, opt => opt.MapFrom(src => CalculateWorkedHours(src.ClockInTime, src.ClockOutTime)));
        }

        private string CalculateWorkedHours(DateTime clockInTime, DateTime clockOutTime)
        {
            // Calculate time difference
            TimeSpan timeDifference = clockOutTime - clockInTime;

            // Calculate total hours and remaining minutes
            int totalHours = (int)timeDifference.TotalHours;
            int remainingMinutes = timeDifference.Minutes;

           
            if (totalHours < 0 || (totalHours == 0 && remainingMinutes < 0))
            {
                
                totalHours = 0;
                remainingMinutes = 0;
            }

            // Format the time difference
            return $"{totalHours} hours {remainingMinutes} minutes";
        }
    
    }
}
