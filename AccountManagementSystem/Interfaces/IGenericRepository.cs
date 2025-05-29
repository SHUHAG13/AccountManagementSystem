using AccountManagementSystem.Models;
using System.Linq.Expressions;

namespace AccountManagementSystem.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<ResponseModel> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<ResponseModel> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<ResponseModel> AddAsync(T entity);
        Task<ResponseModel> UpdateAsync(T entity);
        Task<ResponseModel> DeleteAsync(T entity);
        IQueryable<T> GetQueryable();
    }
}
