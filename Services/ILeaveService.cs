using System.Collections.Generic;
using System.Threading.Tasks;
using Attendance_and_Leave_Management_System.DataModel;

namespace Attendance_and_Leave_Management_System.Services
{
    public interface ILeaveService
    {
        Task CreateRequestAsync(LeaveRequest request);
        Task<IEnumerable<LeaveRequest>> GetEmployeeRequestsAsync(int employeeId);
        Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync();
        Task ApproveRequestAsync(int requestId);
        Task RejectRequestAsync(int requestId);
    }
}