using System.ComponentModel.DataAnnotations;

namespace ReactUser.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DepartmentName { get; set; }
    }
}
