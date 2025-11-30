namespace Attendance_and_Leave_Management_System.ViewModel
{
    public class AttendanceVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public String InStatus { get; set; } = String.Empty;
        public String OutStatus { get; set; } = String.Empty;
        public int TotalHour { get; set; }

    }
}
