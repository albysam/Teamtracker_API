using System.ComponentModel.DataAnnotations;

namespace ReactUser.Models.Dto
{
    public class ApplicationUserUpdateDTO
    {
       
        public string Id { get; set; }
      
        public string Name { get; set; }
       public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile File { get; set; }


        public string Gender { get; set; }
        public string PersonalIdentityNumber { get; set; }

        public int Department { get; set; }

        public int Desgnation { get; set; }

        public string EmploymentType { get; set; }

        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfLeaving { get; set; }
        public string MonthlySalary { get; set; }
        public string Overtime_hourly_Salary { get; set; }
       // public int Status { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string EmployeeId { get; set; }
        public int Shift { get; set; }
    }
}
