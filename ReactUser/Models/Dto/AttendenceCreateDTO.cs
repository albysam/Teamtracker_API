using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactUser.Models.Dto
{
    public class AttendenceCreateDTO
    {
        public string EmployeeId { get; set; }



        public DateTime ClockInTime { get; set; }
       

        public DateTime WorkingDate { get; set; }

        public string WorkedHours { get; set; }

        public string BreakHours { get; set; }




        public int Status { get; set; }
    }
}
