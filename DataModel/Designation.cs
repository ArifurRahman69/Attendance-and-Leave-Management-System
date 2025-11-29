using System.ComponentModel.DataAnnotations;

namespace Attendance_and_Leave_Management_System.DataModel
{
    public class Designation
    {
        [Key]
        public int DesignationId { get; set; }
        public string Name { get; set; }=string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdateAt { get; set; }

    }
}
