using System.ComponentModel.DataAnnotations;

namespace ReactUser.Models.Dto
{
    public class DepartmentUpdateDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DepartmentName { get; set; }
    }
}
