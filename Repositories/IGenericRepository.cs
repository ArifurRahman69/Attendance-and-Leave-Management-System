using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Attendance_and_Leave_Management_System.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<System.Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(object id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);
    }
}