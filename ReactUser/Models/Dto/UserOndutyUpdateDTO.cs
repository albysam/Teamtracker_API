using System.ComponentModel.DataAnnotations;

namespace Talento_API.Models.Dto
{
    public class UserOndutyUpdateDTO
    {

        [Key]
        public int Id { get; set; }
       
        public string UserId { get; set; }

        public DateTime WorkingDate { get; set; }

       
        public DateTime TimeFrom { get; set; }

        public DateTime TimeTo { get; set; }

        public string WorkedHours { get; set; }

        public string Comment { get; set; }

        public int Status { get; set; }

        public string Note { get; set; }
    }

}
