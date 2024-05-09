using System.ComponentModel.DataAnnotations;

namespace Talento_API.Models.Dto
{
    public class UserOvertimeCreateDTO
    {
      

        public string UserId { get; set; }



     

     
        public DateTime OvertimeDate { get; set; }

      
        public DateTime OvertimeFrom { get; set; }

        
        public DateTime OvertimeTo { get; set; }

   

        public string Comment { get; set; }
    }

}
