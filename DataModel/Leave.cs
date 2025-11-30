using System.ComponentModel.DataAnnotations;

namespace Attendance_and_Leave_Management_System.DataModel
{
    public class Leave
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }= string.Empty;
        public string? LeaveType { get; set; }
        public string? LeaveFrom { get; set; }
        public string? LeaveTo { get; set; }
        public string LeaveDuration { get; set; } = string.Empty;
        public string? Status { get; set; } 
    }
}
