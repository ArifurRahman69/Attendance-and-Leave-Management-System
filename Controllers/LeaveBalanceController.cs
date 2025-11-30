using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Attendance_and_Leave_Management_System.Repositories;
using Attendance_and_Leave_Management_System.DataModel;

namespace Attendance_and_Leave_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LeaveBalanceController : Controller
    {
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<LeaveBalance> _leaveBalanceRepository;

        public LeaveBalanceController(IGenericRepository<Employee> employeeRepository, IGenericRepository<LeaveBalance> leaveBalanceRepository)
        {
            _employeeRepository = employeeRepository;
            _leaveBalanceRepository = leaveBalanceRepository;
        }

        // Index: List employees with balances (simple aggregate by description)
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var balances = await _leaveBalanceRepository.GetAllAsync();
            var viewModel = employees.Select(e => new
            {
                Employee = e,
                Balances = balances.ToList()
            });
            return View(viewModel);
        }

        // Edit: Show balances for a specific employee
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();
            var balances = await _leaveBalanceRepository.GetAllAsync();
            ViewBag.Employee = employee;
            return View(balances.ToList());
        }

        // Edit (Post): Update balance (for simplicity, edit by index/id)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveBalance model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _leaveBalanceRepository.UpdateAsync(model);
            TempData["LeaveBalanceMessage"] = "Leave balance updated.";
            return RedirectToAction(nameof(Index));
        }
    }
}