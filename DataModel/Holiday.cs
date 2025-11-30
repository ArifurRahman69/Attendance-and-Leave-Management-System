using System.ComponentModel.DataAnnotations;

namespace Attendance_and_Leave_Management_System.DataModel
{
    public class Holiday
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int HolidayType { get; set; }

        public DateTime DateTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Description { get; set; } = string.Empty;

    }
}
