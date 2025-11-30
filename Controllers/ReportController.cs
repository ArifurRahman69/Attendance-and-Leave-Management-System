using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Attendance_and_Leave_Management_System.Repositories;
using Attendance_and_Leave_Management_System.DataModel;
using Attendance_and_Leave_Management_System.ViewModel;
using System.Collections.Generic;

namespace Attendance_and_Leave_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<Attendance> _attendanceRepository;

        public ReportController(IGenericRepository<Employee> employeeRepository, IGenericRepository<Attendance> attendanceRepository)
        {
            _employeeRepository = employeeRepository;
            _attendanceRepository = attendanceRepository;
        }

        // GET: /Report/MonthlyAttendance?month=1&year=2025
        [HttpGet]
        public async Task<IActionResult> MonthlyAttendance(int month, int year)
        {
            // 1. Get all employees.
            var employees = await _employeeRepository.GetAllAsync();
            var employeeList = employees.ToList();

            // 2. Get all attendance records for the selected month/year.
            var attendances = await _attendanceRepository.GetAllAsync();
            var monthAttendances = attendances
                .Where(a => a.Date.Month == month && a.Date.Year == year)
                .ToList();

            var daysInMonth = DateTime.DaysInMonth(year, month);
            var allDays = Enumerable.Range(1, daysInMonth)
                                    .Select(d => new DateTime(year, month, d))
                                    .ToList();

            // 3. Build report items
            var report = new List<MonthlyAttendanceReportItem>();
            foreach (var emp in employeeList)
            {
                var empRecords = monthAttendances.Where(a => a.EmployeeId == emp.Id).ToList();

                var presentDays = empRecords.Select(r => r.Date.Date).Distinct().Count();
                var lateDays = empRecords.Count(r => string.Equals(r.Status, "Late", StringComparison.OrdinalIgnoreCase));
                var absentDays = allDays.Count - presentDays; // simple absent calculation

                report.Add(new MonthlyAttendanceReportItem
                {
                    EmployeeId = emp.Id,
                    EmployeeName = $"{emp.FirstName} {emp.LastName}",
                    TotalPresentDays = presentDays,
                    TotalAbsentDays = absentDays,
                    TotalLateDays = lateDays
                });
            }

            return View(report);
        }
    }
}