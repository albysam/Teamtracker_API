using System.ComponentModel.DataAnnotations;

namespace ReactUser.Models
{
    public class Leave
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LeaveName { get; set; }

        [Required]
        public string PaidLeaveDays { get; set; }

        public int Status { get; set; }
       
    }
}
