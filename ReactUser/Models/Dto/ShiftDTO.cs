using System.ComponentModel.DataAnnotations;

namespace Talento_API.Models.Dto
{
    public class ShiftDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ShiftName { get; set; }

        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

       [Required]
        public string ShiftDuration { get; set; }

        [Required]
        public string BreakDuration { get; set; }

        public string Notes { get; set; }

        public string TotalShiftDuration { get; set; }
    }
}
