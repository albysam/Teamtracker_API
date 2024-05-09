using System.ComponentModel.DataAnnotations;

namespace ReactUser.Models.Dto
{
    public class DepartmentDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string DepartmentName { get; set; }
    }
}
