using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Attendance_and_Leave_Management_System.Services;
using Attendance_and_Leave_Management_System.Repositories;
using Attendance_and_Leave_Management_System.DataModel;
using Attendance_and_Leave_Management_System.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering; // added

namespace Attendance_and_Leave_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Designation> _designationRepository;
        private readonly IGenericRepository<Shift> _shiftRepository;
        private readonly IGenericRepository<Employee> _employeeRepository;

        public AdminController(
            IEmployeeService employeeService,
            IGenericRepository<Department> departmentRepository,
            IGenericRepository<Designation> designationRepository,
            IGenericRepository<Shift> shiftRepository,
            IGenericRepository<Employee> employeeRepository)
        {
            _employeeService = employeeService;
            _departmentRepository = departmentRepository;
            _designationRepository = designationRepository;
            _shiftRepository = shiftRepository;
            _employeeRepository = employeeRepository;
        }

        // Dashboard
        public async Task<IActionResult> Index()
        {
            var stats = await _employeeService.GetDashboardStatsAsync();
            return View(stats);
        }

        // Employee list
        public async Task<IActionResult> EmployeeList()
        {
            var employees = await _employeeService.GetAllEmployeesWithDetailsAsync();
            return View(employees);
        }

        // GET: Create Employee
        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            await PopulateDropdownsAsync();
            var allEmployees = await _employeeRepository.GetAllAsync();
            var nextEmployeeId = allEmployees.Any() ? allEmployees.Max(e => e.EmployeeID) + 1 : 0;
            return View(new NewEmployeeRegistrationViewModel { EmployeeID = nextEmployeeId });
        }

        // POST: Create Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(NewEmployeeRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(model);
            }

            var result = await _employeeService.RegisterEmployeeAsync(model);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await PopulateDropdownsAsync();
                return View(model);
            }

            TempData["AdminMessage"] = "Employee created successfully.";
            return RedirectToAction(nameof(EmployeeList));
        }

        private async Task PopulateDropdownsAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            var designations = await _designationRepository.GetAllAsync();
            var shifts = await _shiftRepository.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, nameof(Department.Id), nameof(Department.Name));
            ViewBag.Designations = new SelectList(designations, nameof(Designation.Id), nameof(Designation.Name));
            ViewBag.Shifts = new SelectList(shifts, nameof(Shift.Id), nameof(Shift.Name));
        }
    }
}
