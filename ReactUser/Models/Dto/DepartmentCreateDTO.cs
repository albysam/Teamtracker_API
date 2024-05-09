using System.ComponentModel.DataAnnotations;

namespace ReactUser.Models.Dto
{
    public class DepartmentCreateDTO
    {
        [Required]
        public string DepartmentName { get; set; }
    }
}
