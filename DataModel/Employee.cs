using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_and_Leave_Management_System.DataModel
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        // Business serial for employees (auto-generated on create)
        public int EmployeeID { get; set; }

        [Required]
        public string IdentityUserId { get; set; } = string.Empty; // FK to AspNetUsers (ApplicationUser)

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime DateOfJoining { get; set; }

        // Foreign Keys
        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }

        [ForeignKey(nameof(Designation))]
        public int DesignationId { get; set; }

        [ForeignKey("Shift")]
        public int ShiftId { get; set; }

        // Navigation properties
        public ApplicationUser? ApplicationUser { get; set; }
        public Department? Department { get; set; }
        public Designation? Designation { get; set; }
        public Shift? Shift { get; set; }
    }
}