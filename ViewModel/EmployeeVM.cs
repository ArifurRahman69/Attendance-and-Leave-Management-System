namespace Attendance_and_Leave_Management_System.ViewModel
{
    public class EmployeeVM
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = null!;
        public string Password { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
        public String? Address { get; set; }

        public DateTime JoiningDate { get; set; }
        public string BloodGroup { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdateAt { get; set; }
    }
}
