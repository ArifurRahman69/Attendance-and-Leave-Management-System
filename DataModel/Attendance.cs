using System.ComponentModel.DataAnnotations;

namespace Attendance_and_Leave_Management_System.DataModel
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public String InStatus { get; set; } = String.Empty;
        public String OutStatus { get; set; } = String.Empty;
        public float TotalHour { get; set; }

    }
}
