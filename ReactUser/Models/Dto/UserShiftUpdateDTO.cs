using System.ComponentModel.DataAnnotations;

namespace Talento_API.Models.Dto
{
    public class UserShiftUpdateDTO
    {

        [Key]
        public int Id { get; set; }
        public int ShiftId { get; set; }


       


        public string UserId { get; set; }



        public DateTime Date { get; set; }
        [Required]
        public DateTime ShiftDateFrom { get; set; }

        public DateTime ShiftDateTo { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public string ApprovedBy { get; set; }
    }

}
