using System.ComponentModel.DataAnnotations;

namespace Talento_API.Models.Dto
{
    public class UserOvertimeUpdateDTO
    {

        [Key]
        public int Id { get; set; }
       

        public string UserId { get; set; }



      
     
        public DateTime OvertimeDate { get; set; }

        public DateTime OvertimeFrom { get; set; }

    
        public DateTime OvertimeTo { get; set; }


        public string Comment { get; set; }
       
        public string Status { get; set; }

        public string Note { get; set; }

        public string ApprovedBy { get; set; }
    }

}
