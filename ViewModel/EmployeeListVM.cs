namespace Attendance_and_Leave_Management_System.ViewModel
{
    public class EmployeeListVM
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdateAt { get; set; }
    }
}
