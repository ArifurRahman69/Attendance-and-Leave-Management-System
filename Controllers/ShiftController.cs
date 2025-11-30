using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Attendance_and_Leave_Management_System.Repositories;
using Attendance_and_Leave_Management_System.DataModel;

namespace Attendance_and_Leave_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShiftController : Controller
    {
        private readonly IGenericRepository<Shift> _shiftRepository;

        public ShiftController(IGenericRepository<Shift> shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        // GET: /Shift
        public async Task<IActionResult> Index()
        {
            var shifts = await _shiftRepository.GetAllAsync();
            return View(shifts);
        }

        // GET: /Shift/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Shift/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Shift model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _shiftRepository.AddAsync(model);
            TempData["ShiftMessage"] = "Shift created.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Shift/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var shift = await _shiftRepository.GetByIdAsync(id);
            if (shift == null) return NotFound();
            return View(shift);
        }

        // POST: /Shift/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Shift model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            await _shiftRepository.UpdateAsync(model);
            TempData["ShiftMessage"] = "Shift updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Shift/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var shift = await _shiftRepository.GetByIdAsync(id);
            if (shift == null) return NotFound();
            return View(shift);
        }

        // POST: /Shift/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _shiftRepository.DeleteAsync(id);
            TempData["ShiftMessage"] = "Shift deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}