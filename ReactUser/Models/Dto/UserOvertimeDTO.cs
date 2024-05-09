using ReactUser.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talento_API.Models.Dto
{
    public class UserOvertimeDTO
    {
       
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
        [Required]

        public int Status { get; set; }

        public string Note { get; set; }

       
        // New properties
        public string UserName { get; set; }
        public string Name { get; set; }
       
    }
}
