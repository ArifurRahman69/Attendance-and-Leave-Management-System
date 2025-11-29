using System.ComponentModel.DataAnnotations;

namespace Attendance_and_Leave_Management_System.DataModel
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
      public string? Street { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdateAt { get; set; }
    }
}
