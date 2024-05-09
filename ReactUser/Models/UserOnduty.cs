using ReactUser.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talento_API.Models
{
    public class UserOnduty
    {
        [Key]
        public int Id { get; set; }

       

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

      

        [Required]
        public DateTime WorkingDate { get; set; }

        [Required]
        public DateTime TimeFrom { get; set; }

        [Required]
        public DateTime TimeTo { get; set; }

        public string WorkedHours { get; set; }



        [Required]

        public string Comment { get; set; }

      

        public int Status { get; set; }
       
        public string Note { get; set; }

      

    }
}