using AccountManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using MiniAccountApi.Data;
using MiniAccountApi.Models;

namespace AccountManagementSystem.Services
{
    public class ChartOfAccountRepository: IChartOfAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public ChartOfAccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChartOfAccount>> GetRootAccountsAsync()
        {
            return await _context.ChartOfAccounts
                .Include(c => c.Children)
                .Where(c => c.ParentId == null)
                .ToListAsync();
        }

        public async Task<ChartOfAccount?> GetByIdAsync(int id)
        {
            return await _context.ChartOfAccounts
                .Include(c => c.Children)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(ChartOfAccount account)
        {
            await _context.ChartOfAccounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChartOfAccount account)
        {
            _context.ChartOfAccounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var account = await _context.ChartOfAccounts.FindAsync(id);
            if (account != null)
            {
                _context.ChartOfAccounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }
}
