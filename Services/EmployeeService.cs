using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Attendance_and_Leave_Management_System.DataModel;
using Attendance_and_Leave_Management_System.Repositories;
using Attendance_and_Leave_Management_System.ViewModel;

namespace Attendance_and_Leave_Management_System.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<Attendance> _attendanceRepository;
        private readonly IGenericRepository<LeaveRequest> _leaveRepository;
        private readonly IGenericRepository<LeaveBalance> _leaveBalanceRepository;

        public EmployeeService(
            UserManager<ApplicationUser> userManager,
            IGenericRepository<Employee> employeeRepository,
            IGenericRepository<Attendance> attendanceRepository,
            IGenericRepository<LeaveRequest> leaveRepository,
            IGenericRepository<LeaveBalance> leaveBalanceRepository)
        {
            _userManager = userManager;
            _employeeRepository = employeeRepository;
            _attendanceRepository = attendanceRepository;
            _leaveRepository = leaveRepository;
            _leaveBalanceRepository = leaveBalanceRepository;
        }

        public async Task<IdentityResult> RegisterEmployeeAsync(NewEmployeeRegistrationViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return result; // return errors to caller
            }

            // Attempt to add to Employee role (assumes role exists)
            try
            {
                await _userManager.AddToRoleAsync(user, "Employee");
            }
            catch
            {
                // swallow; role might not exist yet
            }

            // Determine EmployeeID: use provided value if any; else auto-generate
            var existingEmployees = await _employeeRepository.GetAllAsync();
            int nextEmployeeId = existingEmployees.Any() ? existingEmployees.Max(e => e.EmployeeID) + 1 : 0;
            int finalEmployeeId = model.EmployeeID.HasValue && model.EmployeeID.Value >= 0 ? model.EmployeeID.Value : nextEmployeeId;

            // Create Employee domain entity
            var employee = new Employee
            {
                IdentityUserId = user.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                DateOfJoining = DateTime.UtcNow,
                DepartmentId = model.DepartmentId,
                DesignationId = model.DesignationId,
                ShiftId = model.ShiftId,
                EmployeeID = finalEmployeeId
            };

            await _employeeRepository.AddAsync(employee);

            // Add default LeaveBalance records for the new employee
            var defaultBalances = new []
            {
                new LeaveBalance { Description = "Sick Leave", Balance = 14 },
                new LeaveBalance { Description = "Casual Leave", Balance = 10 },
                new LeaveBalance { Description = "Annual Leave", Balance = 14 }
            };
            foreach (var lb in defaultBalances)
            {
                // If LeaveBalance should be tied to Employee, add fields accordingly (e.g., EmployeeId)
                await _leaveBalanceRepository.AddAsync(lb);
            }

            return result;
        }

        public async Task<AdminDashboardViewModel> GetDashboardStatsAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var attendances = await _attendanceRepository.GetAllAsync();
            var leaves = await _leaveRepository.GetAllAsync();

            var today = DateTime.UtcNow.Date;

            var totalEmployees = employees.Count();
            var employeesPresentToday = attendances
                .Where(a => a.Date.Date == today && a.InTime != null)
                .Select(a => a.EmployeeId)
                .Distinct()
                .Count();

            var pendingLeaveRequests = leaves.Count(l => l.Status == LeaveStatus.Pending);

            return new AdminDashboardViewModel
            {
                TotalEmployees = totalEmployees,
                EmployeesPresentToday = employeesPresentToday,
                PendingLeaveRequests = pendingLeaveRequests
            };
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync()
        {
            var items = await _employeeRepository.GetAllAsync(e => e.Department!, e => e.Designation!);
            return items;
        }
    }
}