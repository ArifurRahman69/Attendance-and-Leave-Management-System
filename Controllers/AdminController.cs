using Microsoft.AspNetCore.Mvc;

namespace Attendance_and_Leave_Management_System.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
