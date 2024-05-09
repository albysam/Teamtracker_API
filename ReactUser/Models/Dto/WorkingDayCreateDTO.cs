using System.ComponentModel.DataAnnotations;

namespace Talento_API.Models.Dto
{
    public class WorkingDayCreateDTO
    {
        [Required]
        public int WorkingDays { get; set; }

        public string Status { get; set; }
    }
}
