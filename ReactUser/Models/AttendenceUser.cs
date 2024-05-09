using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReactUser.Models
{
    public class AttendenceUser
    {
        [Key]
        public int Id { get; set; }

       
        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public ApplicationUser User { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime ClockOutTime { get; set; }
        public DateTime BreakStartTime { get; set; }
        public DateTime BreakEndTime { get; set; }

        public DateTime WorkingDate { get; set; }

        public string WorkedHours { get; set; }

        public string BreakHours { get; set; }


      

        public int Status { get; set; }
       
    }
}
