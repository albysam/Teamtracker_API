using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactUser.Models;
using Talento_API;
using Talento_API.Models;

namespace ReactUser.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Desgnation> DesgnationsType { get; set; }

        public DbSet<Leave> Leaves { get; set; }
        public DbSet<UserLeave> UserLeaves { get; set; }
        public DbSet<Resgnation> Resgnations { get; set; }
     
        public DbSet<AttendenceUser> AttendenceUsers { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<UserShift> UserShifts { get; set; }
        public DbSet<UserOvertime> UserOvertimes { get; set; }

        public DbSet<UserOnduty> UserOndutys { get; set; }
        public DbSet<WorkingDay> WorkingDays { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Department>().HasData(new Department
            {
                Id = 1,
                DepartmentName = "Manager"

            }, new Department
            {
                Id = 2,
                DepartmentName = "Developer"
            });
            

        }

        }
    }

