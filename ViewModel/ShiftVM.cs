namespace Attendance_and_Leave_Management_System.ViewModel
{
    public class ShiftVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? WorkHour { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
    }
}
