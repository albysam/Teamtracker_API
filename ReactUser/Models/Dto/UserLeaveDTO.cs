using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactUser.Models.Dto
{
    public class UserLeaveDTO
    {
       
        public int Id { get; set; }

      
       
        public int LeaveId { get; set; }

        [ForeignKey("LeaveId")]
        public Leave Leave { get; set; }

      
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public string LeaveDays { get; set; }

        public string ApprovedStatus { get; set; }

        public string Comment { get; set; }

        public int Status { get; set; }
        [Required]
        public string Type { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }

        public string ApprovedBy { get; set; }

        [Required]
        public string Reason { get; set; }
        // New properties
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LeaveName { get; set; }
    }
}
