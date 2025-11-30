using System.ComponentModel.DataAnnotations;

namespace Attendance_and_Leave_Management_System.DataModel
{
    public class Shift
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? WorkHour { get; set; }   
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
    }
}
