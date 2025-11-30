using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Attendance_and_Leave_Management_System.DataModel
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation collection of employees in this department
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
