using ReactUser.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Desgnation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [ForeignKey("DepartmentId")]
    public Department Department { get; set; }  

    [Required]
    public string DesgnationName { get; set; }
}
