using System.ComponentModel.DataAnnotations;

namespace ReactUser.Models.Dto
{
    public class DesgnationCreateDTO
    {
        //public List<string> DepartmentNames { get; set; } // Add this property

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string DesgnationName { get; set; }
    }

}
