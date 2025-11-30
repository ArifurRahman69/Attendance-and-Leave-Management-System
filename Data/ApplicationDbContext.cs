using Attendance_and_Leave_Management_System.DataModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Attendance_and_Leave_Management_System.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Department> Departments { get; set; }
        
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<Shift> Shifts { get; set; }

    }
}
