using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attendance_and_Leave_Management_System.DataModel;
using Attendance_and_Leave_Management_System.Repositories;

namespace Attendance_and_Leave_Management_System.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IGenericRepository<Attendance> _attendanceRepository;
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<Holiday> _holidayRepository;

        public AttendanceService(IGenericRepository<Attendance> attendanceRepository, IGenericRepository<Employee> employeeRepository, IGenericRepository<Holiday> holidayRepository)
        {
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
            _holidayRepository = holidayRepository;
        }

        public async Task<string> ClockInAsync(int employeeId)
        {
            var all = await _attendanceRepository.GetAllAsync();
            var today = DateTime.UtcNow.Date;

            // Weekend check (Friday/Saturday)
            if (today.DayOfWeek == DayOfWeek.Friday || today.DayOfWeek == DayOfWeek.Saturday)
            {
                return "Weekend";
            }

            // Holiday check
            var holidays = await _holidayRepository.GetAllAsync();
            var matchedHoliday = holidays.FirstOrDefault(h => h.StartDate.Date <= today && h.EndDate.Date >= today);
            if (matchedHoliday != null)
            {
                return $"Cannot Clock-In: Today is a Holiday ({matchedHoliday.Name})";
            }

            var todayRecord = all.FirstOrDefault(a => a.EmployeeId == employeeId && a.Date.Date == today);

            if (todayRecord != null)
            {
                if (todayRecord.InTime != null && todayRecord.OutTime == null)
                {
                    return "Already clocked in.";
                }
                if (todayRecord.InTime != null && todayRecord.OutTime != null)
                {
                    return "Already clocked in and out for today.";
                }
            }

            var now = DateTime.UtcNow;

            // Retrieve Employee's assigned Shift and mark late if needed
            var employee = (await _employeeRepository.GetAllAsync()).FirstOrDefault(e => e.Id == employeeId);
            string? status = null;
            if (employee?.Shift != null && !string.IsNullOrWhiteSpace(employee.Shift.StartTime))
            {
                if (TimeSpan.TryParse(employee.Shift.StartTime, out var shiftStart))
                {
                    var currentLocalTime = now.TimeOfDay;
                    var grace = TimeSpan.FromMinutes(15);
                    if (currentLocalTime > (shiftStart + grace))
                    {
                        status = "Late";
                    }
                }
            }

            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                Date = today,
                InTime = now,
                TotalHours = 0d,
                Status = status
            };
            await _attendanceRepository.AddAsync(attendance);
            return $"Clock-in successful at {now:HH:mm:ss} UTC" + (status == "Late" ? " (Late)" : string.Empty);
        }

        public async Task<string> ClockOutAsync(int employeeId)
        {
            var all = await _attendanceRepository.GetAllAsync();
            var today = DateTime.UtcNow.Date;
            var todayRecord = all.FirstOrDefault(a => a.EmployeeId == employeeId && a.Date.Date == today && a.InTime != null && a.OutTime == null);

            if (todayRecord == null)
            {
                return "No active clock-in found for today.";
            }

            var now = DateTime.UtcNow;
            todayRecord.OutTime = now;
            if (todayRecord.InTime != null)
            {
                todayRecord.TotalHours = Math.Round((now - todayRecord.InTime.Value).TotalHours, 2);
            }
            await _attendanceRepository.UpdateAsync(todayRecord);
            return $"Clock-out successful at {now:HH:mm:ss} UTC. Total hours: {todayRecord.TotalHours}";
        }

        public async Task<IEnumerable<Attendance>> GetHistoryAsync(int employeeId)
        {
            var all = await _attendanceRepository.GetAllAsync();
            return all.Where(a => a.EmployeeId == employeeId)
                      .OrderByDescending(a => a.Date)
                      .ThenByDescending(a => a.InTime);
        }

        public async Task<bool> IsCheckedInAsync(int employeeId)
        {
            var all = await _attendanceRepository.GetAllAsync();
            var today = DateTime.UtcNow.Date;
            return all.Any(a => a.EmployeeId == employeeId && a.Date.Date == today && a.InTime != null && a.OutTime == null);
        }
    }
}