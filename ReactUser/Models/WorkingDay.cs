using System.ComponentModel.DataAnnotations;

namespace Talento_API.Models
{
    public class WorkingDay
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int WorkingDays { get; set; }
        
        public string Status { get; set; }
    }
}
