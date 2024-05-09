using System.ComponentModel.DataAnnotations;

namespace Talento_API.Models.Dto
{
    public class WorkingDayUpdateDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int WorkingDays { get; set; }

        public string Status { get; set; }
    }
}
