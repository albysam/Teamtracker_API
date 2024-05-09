using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactUser.Models
{
    public class Resgnation
    {
        [Key]
        public int Id { get; set; }

       
        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime ResgnationDate { get; set; }

        [Required]
        public string Reason { get; set; }

        public string Comment { get; set; }

        public int Status { get; set; }
       
    }
}
