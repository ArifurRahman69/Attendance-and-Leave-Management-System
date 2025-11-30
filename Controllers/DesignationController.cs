using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Attendance_and_Leave_Management_System.Repositories;
using Attendance_and_Leave_Management_System.DataModel;

namespace Attendance_and_Leave_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DesignationController : Controller
    {
        private readonly IGenericRepository<Designation> _designationRepository;

        public DesignationController(IGenericRepository<Designation> designationRepository)
        {
            _designationRepository = designationRepository;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _designationRepository.GetAllAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Designation model)
        {
            if (!ModelState.IsValid) return View(model);
            await _designationRepository.AddAsync(model);
            TempData["DesigMessage"] = "Designation created.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _designationRepository.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Designation model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            await _designationRepository.UpdateAsync(model);
            TempData["DesigMessage"] = "Designation updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _designationRepository.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _designationRepository.DeleteAsync(id);
            TempData["DesigMessage"] = "Designation deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}