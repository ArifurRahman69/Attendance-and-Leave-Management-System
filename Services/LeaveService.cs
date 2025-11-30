using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attendance_and_Leave_Management_System.DataModel;
using Attendance_and_Leave_Management_System.Repositories;

namespace Attendance_and_Leave_Management_System.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly IGenericRepository<LeaveRequest> _leaveRepository;
        private readonly IGenericRepository<LeaveBalance> _leaveBalanceRepository;

        public LeaveService(IGenericRepository<LeaveRequest> leaveRepository, IGenericRepository<LeaveBalance> leaveBalanceRepository)
        {
            _leaveRepository = leaveRepository;
            _leaveBalanceRepository = leaveBalanceRepository;
        }

        public async Task CreateRequestAsync(LeaveRequest request)
        {
            request.Status = LeaveStatus.Pending;
            request.RequestDate = System.DateTime.UtcNow;
            await _leaveRepository.AddAsync(request);
        }

        public async Task<IEnumerable<LeaveRequest>> GetEmployeeRequestsAsync(int employeeId)
        {
            var all = await _leaveRepository.GetAllAsync();
            return all.Where(r => r.EmployeeId == employeeId)
                      .OrderByDescending(r => r.RequestDate);
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync()
        {
            var all = await _leaveRepository.GetAllAsync();
            return all.Where(r => r.Status == LeaveStatus.Pending)
                      .OrderBy(r => r.StartDate);
        }

        public async Task ApproveRequestAsync(int requestId)
        {
            // 1. Get the LeaveRequest by ID.
            var request = await _leaveRepository.GetByIdAsync(requestId);
            if (request == null) return;
            if (request.Status != LeaveStatus.Pending) return;

            // 2. Calculate total days (EndDate - StartDate).
            var totalDays = (int)System.Math.Ceiling((request.EndDate.Date - request.StartDate.Date).TotalDays + 1);
            if (totalDays <= 0) totalDays = 1;

            // 3. Retrieve the LeaveBalance record for this Employee and LeaveType.
            var balances = await _leaveBalanceRepository.GetAllAsync();
            var balance = balances.FirstOrDefault(b => b.Description == request.LeaveType && b != null);
            // If your schema associates LeaveBalance to Employee, adjust filter: b.EmployeeId == request.EmployeeId && b.Type == request.LeaveType

            // 4. Check if Balance >= total days. If not, return without approving.
            if (balance == null || !balance.Balance.HasValue || balance.Balance.Value < totalDays)
            {
                // Could throw or signal error; here we just return without changing status.
                return;
            }

            // 5. Subtract total days from Balance and update LeaveBalance table.
            balance.Balance = balance.Balance.Value - totalDays;
            await _leaveBalanceRepository.UpdateAsync(balance);

            // 6. Finally, update LeaveRequest.Status to Approved.
            request.Status = LeaveStatus.Approved;
            await _leaveRepository.UpdateAsync(request);
        }

        public async Task RejectRequestAsync(int requestId)
        {
            var request = await _leaveRepository.GetByIdAsync(requestId);
            if (request == null) return;
            if (request.Status == LeaveStatus.Pending)
            {
                request.Status = LeaveStatus.Rejected;
                await _leaveRepository.UpdateAsync(request);
            }
        }
    }
}