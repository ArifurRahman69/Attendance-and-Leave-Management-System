using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Attendance_and_Leave_Management_System.Repositories;
using Attendance_and_Leave_Management_System.DataModel;

namespace Attendance_and_Leave_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly IGenericRepository<Employee> _employeeRepository;

        public EmployeeController(IGenericRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: /Employee/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // GET: /Employee/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // GET: /Employee/NewEmployee
        [HttpGet]
        public IActionResult NewEmployee()
        {
            return View();
        }

        // GET: /Employee/EmployeeList
        [HttpGet]
        public IActionResult EmployeeList()
        {
            return View();
        }
    }
}