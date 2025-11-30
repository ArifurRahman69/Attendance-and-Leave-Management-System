using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Attendance_and_Leave_Management_System.DataModel;
using Attendance_and_Leave_Management_System.Services;
using Attendance_and_Leave_Management_System.Repositories;
using System.Linq;

namespace Attendance_and_Leave_Management_System.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<LeaveBalance> _leaveBalanceRepository;

        public AttendanceController(IAttendanceService attendanceService, UserManager<ApplicationUser> userManager, IGenericRepository<LeaveBalance> leaveBalanceRepository)
        {
            _attendanceService = attendanceService;
            _userManager = userManager;
            _leaveBalanceRepository = leaveBalanceRepository;
        }

        // GET: /Attendance
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!(User?.Identity?.IsAuthenticated ?? false))
            {
                return Unauthorized();
            }

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            var employeeId = await GetCurrentEmployeeIdAsync();
            if (employeeId == null)
            {
                return Unauthorized();
            }

            // Today's status
            var isCheckedIn = await _attendanceService.IsCheckedInAsync(employeeId.Value);
            var history = await _attendanceService.GetHistoryAsync(employeeId.Value);
            var today = System.DateTime.UtcNow.Date;
            var todayRecord = history.FirstOrDefault(a => a.Date.Date == today);
            var status = todayRecord == null ? "Not Clocked In"
                        : todayRecord.InTime != null && todayRecord.OutTime == null ? "Clocked In"
                        : todayRecord.InTime != null && todayRecord.OutTime != null ? "Completed"
                        : "Unknown";
            if (!string.IsNullOrEmpty(todayRecord?.Status))
            {
                status = todayRecord.Status;
            }

            // Leave balance summary (simple aggregate)
            var balances = await _leaveBalanceRepository.GetAllAsync();
            var totalBalance = balances.Where(b => b.Balance.HasValue).Sum(b => b.Balance!.Value);

            ViewBag.TodayStatus = status;
            ViewBag.TotalLeaveBalance = totalBalance;

            return View(history);
        }

        // POST: /Attendance/ClockIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClockIn()
        {
            var employeeId = await GetCurrentEmployeeIdAsync();
            if (employeeId == null)
            {
                return Unauthorized();
            }
            var message = await _attendanceService.ClockInAsync(employeeId.Value);
            TempData["AttendanceMessage"] = message;
            return RedirectToAction(nameof(Index));
        }

        // POST: /Attendance/ClockOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClockOut()
        {
            var employeeId = await GetCurrentEmployeeIdAsync();
            if (employeeId == null)
            {
                return Unauthorized();
            }
            var message = await _attendanceService.ClockOutAsync(employeeId.Value);
            TempData["AttendanceMessage"] = message;
            return RedirectToAction(nameof(Index));
        }

        // Helper: Try to get current EmployeeId from claims
        private async Task<int?> GetCurrentEmployeeIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return null;
            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;
            if (int.TryParse(employeeIdClaim, out var employeeId))
            {
                return employeeId;
            }
            // Fallback: if claim not present, cannot resolve without repository/context
            return null;
        }
    }
}