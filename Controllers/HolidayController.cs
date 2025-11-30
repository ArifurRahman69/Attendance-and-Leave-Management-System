using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Attendance_and_Leave_Management_System.Repositories;
using Attendance_and_Leave_Management_System.DataModel;

namespace Attendance_and_Leave_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HolidayController : Controller
    {
        private readonly IGenericRepository<Holiday> _holidayRepository;

        public HolidayController(IGenericRepository<Holiday> holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        // 1. Index: List all holidays ordered by StartDate descending.
        public async Task<IActionResult> Index()
        {
            var holidays = await _holidayRepository.GetAllAsync();
            var ordered = holidays.OrderByDescending(h => h.StartDate);
            return View(ordered);
        }

        // 2. Create (Get)
        [HttpGet]
        public IActionResult Create()
        {
            var today = System.DateTime.UtcNow.Date;
            var model = new Holiday
            {
                StartDate = today,
                EndDate = today
            };
            return View(model);
        }

        // 2. Create (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Holiday model)
        {
            if (model.StartDate > model.EndDate)
            {
                ModelState.AddModelError(nameof(Holiday.EndDate), "End date must be greater than or equal to start date.");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _holidayRepository.AddAsync(model);
            TempData["HolidayMessage"] = "Holiday created.";
            return RedirectToAction(nameof(Index));
        }

        // 3. Edit (Get)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var holiday = await _holidayRepository.GetByIdAsync(id);
            if (holiday == null) return NotFound();
            return View(holiday);
        }

        // 3. Edit (Post)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Holiday model)
        {
            if (id != model.Id) return BadRequest();
            if (model.StartDate > model.EndDate)
            {
                ModelState.AddModelError(nameof(Holiday.EndDate), "End date must be greater than or equal to start date.");
            }
            if (!ModelState.IsValid) return View(model);
            await _holidayRepository.UpdateAsync(model);
            TempData["HolidayMessage"] = "Holiday updated.";
            return RedirectToAction(nameof(Index));
        }

        // 4. Delete (Get)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var holiday = await _holidayRepository.GetByIdAsync(id);
            if (holiday == null) return NotFound();
            return View(holiday);
        }

        // 4. Delete (Post)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _holidayRepository.DeleteAsync(id);
            TempData["HolidayMessage"] = "Holiday deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}