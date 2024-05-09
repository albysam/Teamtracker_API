using ReactUser.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talento_API.Models
{
    public class UserOvertime
    {
        [Key]
        public int Id { get; set; }

       

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

      

        [Required]
        public DateTime OvertimeDate { get; set; }

        [Required]
        public DateTime OvertimeFrom { get; set; }

        [Required]
        public DateTime OvertimeTo { get; set; }

        [Required]

        public string Comment { get; set; }

      

        public int Status { get; set; }
       
        public string Note { get; set; }

      

    }
}