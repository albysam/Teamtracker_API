using ReactUser.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Talento_API.Models
{
    public class Holiday
    {
        [Key]
        public int Id { get; set; }
        public string HolidayName { get; set; }

        [Required]
        public DateTime Date { get; set; }
        
        public string Comment { get; set; }
        public string Status { get; set; }
        
       
       
    }
}