using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Attendance_and_Leave_Management_System.Repositories;
using Attendance_and_Leave_Management_System.DataModel;

namespace Attendance_and_Leave_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IGenericRepository<Department> _departmentRepository;

        public DepartmentController(IGenericRepository<Department> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        // GET: /Department
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return View(departments);
        }

        // GET: /Department/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _departmentRepository.AddAsync(model);
            TempData["DeptMessage"] = "Department created.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Department/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null) return NotFound();
            return View(department);
        }

        // POST: /Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            await _departmentRepository.UpdateAsync(model);
            TempData["DeptMessage"] = "Department updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Department/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null) return NotFound();
            return View(department);
        }

        // POST: /Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departmentRepository.DeleteAsync(id);
            TempData["DeptMessage"] = "Department deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}