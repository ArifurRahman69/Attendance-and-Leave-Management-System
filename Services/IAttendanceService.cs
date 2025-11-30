using System.Collections.Generic;
using System.Threading.Tasks;
using Attendance_and_Leave_Management_System.DataModel;

namespace Attendance_and_Leave_Management_System.Services
{
    public interface IAttendanceService
    {
        Task<string> ClockInAsync(int employeeId);
        Task<string> ClockOutAsync(int employeeId);
        Task<IEnumerable<Attendance>> GetHistoryAsync(int employeeId);
        Task<bool> IsCheckedInAsync(int employeeId);
    }
}