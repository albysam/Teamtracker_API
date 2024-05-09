using System.ComponentModel.DataAnnotations;

namespace ReactUser.Models.Dto
{
    public class LeaveCreateDTO
    {
        [Required]
        public string LeaveName { get; set; }

        [Required]
        public string PaidLeaveDays { get; set; }

        public int Status { get; set; }
    }
}
