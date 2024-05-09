using ReactUser.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talento_API.Models
{
    public class UserShift
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public int ShiftId { get; set; }

        [ForeignKey("ShiftId")]
        public Shift Shift { get; set; }

      

        
        public DateTime Date { get; set; }
        [Required]
        public DateTime ShiftDateFrom { get; set; }
       
        public DateTime ShiftDateTo { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public string ApprovedBy { get; set; }

       
       
    }
}