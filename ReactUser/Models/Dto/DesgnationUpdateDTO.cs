using System.ComponentModel.DataAnnotations;
namespace ReactUser.Models.Dto
{
    public class DesgnationUpdateDTO
    {
       
        [Key]
        public int Id { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string DesgnationName { get; set; }
    }
}