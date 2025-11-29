namespace Attendance_and_Leave_Management_System.ViewModel
{
    public class DepartmentVM
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdateAt { get; set; }
    }
}
