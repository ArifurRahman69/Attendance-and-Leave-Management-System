using System.ComponentModel.DataAnnotations;

namespace Attendance_and_Leave_Management_System.DataModel
{
    public class LeaveBalance
    {
        [Key]
        public int Id { get; set; }
        public string? RemainingTime { get; set; }
        public string? Description { get; set; }
        public int? Balance { get; set; }
    }
}
