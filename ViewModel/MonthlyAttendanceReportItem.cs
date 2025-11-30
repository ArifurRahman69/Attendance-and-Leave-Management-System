namespace Attendance_and_Leave_Management_System.ViewModel
{
    public class MonthlyAttendanceReportItem
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int TotalPresentDays { get; set; }
        public int TotalAbsentDays { get; set; }
        public int TotalLateDays { get; set; }
    }
}