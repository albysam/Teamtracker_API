using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactUser.Models.Dto
{
    public class ResgnationUpdateDTO
    {
        [Key]
        public int Id { get; set; }


        public string EmployeeId { get; set; }

       

        [Required]
        public DateTime ResgnationDate { get; set; }

        [Required]
        public string Reason { get; set; }

        public string Comment { get; set; }

        public int Status { get; set; }
    }
}
