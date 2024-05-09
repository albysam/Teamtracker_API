using System.ComponentModel.DataAnnotations;

namespace ReactUser.Models.Dto
{
    public class LeaveDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string LeaveName { get; set; }

        [Required]
        public string PaidLeaveDays { get; set; }

        public int Status { get; set; }
    }
}
