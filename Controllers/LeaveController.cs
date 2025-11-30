using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Attendance_and_Leave_Management_System.DataModel;
using Attendance_and_Leave_Management_System.Services;
using Attendance_and_Leave_Management_System.ViewModel;
using Attendance_and_Leave_Management_System.Repositories;

namespace Attendance_and_Leave_Management_System.Controllers
{
    [Authorize]
    public class LeaveController : Controller
    {
        private readonly ILeaveService _leaveService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<Employee> _employeeRepo;

        public LeaveController(
            ILeaveService leaveService,
            UserManager<ApplicationUser> userManager,
            IGenericRepository<Employee> employeeRepo)
        {
            _leaveService = leaveService;
            _userManager = userManager;
            _employeeRepo = employeeRepo;
        }

        // GET: /Leave/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new LeaveRequest());
        }

        // POST: /Leave/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequest model)
        {
            // Remove Employee validation because we set it manually
            ModelState.Remove("Employee");

            if (!ModelState.IsValid) return View(model);

            var employeeId = await GetCurrentEmployeeIdAsync();
            if (employeeId == null) return Unauthorized();

            model.EmployeeId = employeeId.Value;
            await _leaveService.CreateRequestAsync(model);
            TempData["LeaveMessage"] = "Leave request submitted.";
            return RedirectToAction(nameof(MyLeaves));
        }

        // GET: /Leave/MyLeaves
        [HttpGet]
        public async Task<IActionResult> MyLeaves()
        {
            var employeeId = await GetCurrentEmployeeIdAsync();
            if (employeeId == null) return Unauthorized();

            var requests = await _leaveService.GetEmployeeRequestsAsync(employeeId.Value);
            var vm = requests.Select(r => new LeaveVM
            {
                Id = r.Id,
                LeaveType = r.LeaveType,
                LeaveFrom = r.StartDate.ToShortDateString(),
                LeaveTo = r.EndDate.ToShortDateString(),
                LeaveDuration = ((r.EndDate - r.StartDate).TotalDays + 1).ToString(),
                Status = r.Status.ToString()
            });
            return View(vm);
        }

        // GET: /Leave/AdminList
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AdminList()
        {
            var pending = await _leaveService.GetPendingRequestsAsync();
            return View(pending);
        }

        // POST: Approve
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            await _leaveService.ApproveRequestAsync(id);
            return RedirectToAction(nameof(AdminList));
        }

        // POST: Reject
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            await _leaveService.RejectRequestAsync(id);
            return RedirectToAction(nameof(AdminList));
        }

        private async Task<int?> GetCurrentEmployeeIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return null;

            var employees = await _employeeRepo.GetAllAsync();
            var employee = employees.FirstOrDefault(e => e.IdentityUserId == user.Id);
            return employee?.Id;
        }
    }
}