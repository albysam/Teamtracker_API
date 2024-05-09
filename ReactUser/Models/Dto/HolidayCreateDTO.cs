using System.ComponentModel.DataAnnotations;

namespace Talento_API.Models.Dto
{
    public class HolidayCreateDTO
    {
        [Required]
        public string HolidayName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Comment { get; set; }
        public string Status { get; set; }
    }
}
