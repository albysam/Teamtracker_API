namespace ReactUser.Utility
{

    public enum UserRole
    {
        Admin,
        Employee,
        SuperAdmin
    }


    public class SD
    {

        public const string Role_Admin = "admin";
        public const string Role_Employee = "employee";
      public const string Role_SuperAdmin= "super admin";
       // public const string SD_Storage_Container = "talentusers";
        public const string SD_Storage_Container = "talento";

        public const string Gender_Male = "male";
        public const string Gender_Female = "female";
        public const string Employment_Permanent= "permanent";
        public const string Employment_Temporary = "temporary";

        public const string Leave_Fullday = "full day";
        public const string Leave_Halfday = "half day";

        public const int Resgnation_Pending = 0;
        public const int Resgnation_Approved = 1;
        public const int Resgnation_NotApproved = 2;

        public const int Leave_Pending = 0;
        public const int Leave_Approved = 1;
        public const int Leave_NotApproved = 2;

        public const int Overtime_Pending = 0;
        public const int Overtime_Approved = 1;
        public const int Overtime_NotApproved = 2;

        public const int Onduty_Pending = 0;
        public const int Onduty_Approved = 1;
        public const int Onduty_NotApproved = 2;


    }
}
