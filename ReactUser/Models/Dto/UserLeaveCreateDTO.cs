using System.ComponentModel.DataAnnotations;

namespace ReactUser.Models.Dto
{
    public class UserLeaveCreateDTO
    {
        public int LeaveId { get; set; }


       


        public string UserId { get; set; }


       
        public string LeaveDays { get; set; }

        public string ApprovedStatus { get; set; }
        public string Comment { get; set; }

      
        public string Type { get; set; }

       
        public DateTime DateFrom { get; set; }

       
        public DateTime DateTo { get; set; }

        public string Reason { get; set; }
    }

}
