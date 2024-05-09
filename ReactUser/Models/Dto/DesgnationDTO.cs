using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactUser.Models.Dto
{
    public class DesgnationDTO
    {
        public int Id { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }  // Navigation property for Department

        [Required]
        public string DesgnationName { get; set; }
    }
}
