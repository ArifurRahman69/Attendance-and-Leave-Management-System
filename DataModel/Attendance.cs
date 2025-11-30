using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_and_Leave_Management_System.DataModel
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }

        public double TotalHours { get; set; }

        public string? Status { get; set; } // e.g., Late, OnTime

        // Navigation property
        public Employee? Employee { get; set; }
    }
}