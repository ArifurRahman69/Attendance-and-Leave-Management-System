using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_and_Leave_Management_System.DataModel
{
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LeaveType { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Required]
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        // Navigation property
        public Employee Employee { get; set; }
    }
}