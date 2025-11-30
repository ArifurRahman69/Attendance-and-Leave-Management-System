using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Attendance_and_Leave_Management_System.DataModel;
using Attendance_and_Leave_Management_System.ViewModel;

namespace Attendance_and_Leave_Management_System.Services
{
    public interface IEmployeeService
    {
        Task<IdentityResult> RegisterEmployeeAsync(NewEmployeeRegistrationViewModel model);
        Task<AdminDashboardViewModel> GetDashboardStatsAsync();
        Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync();
    }
}