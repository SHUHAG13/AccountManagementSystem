using AccountManagementSystem.Interfaces;
using AccountManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using MiniAccountApi.Data;
using System.Linq.Expressions;

namespace AccountManagementSystem.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<ResponseModel> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                var result = await query.ToListAsync();
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Data fetched successfully.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                var entity = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

                if (entity == null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Entity not found."
                    };
                }

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Entity fetched successfully.",
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Entity added successfully.",
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Entity updated successfully.",
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseModel> DeleteAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Entity deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}
