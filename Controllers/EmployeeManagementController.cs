using Microsoft.AspNetCore.Mvc;

namespace Attendance_and_Leave_Management_System.Controllers
{
    public class EmployeeManagementController : Controller
    {
        public IActionResult NewEmployee()
        {
            return View();
        }



        public IActionResult EmployeeList()
        {
            return View();
        }
    }
}
