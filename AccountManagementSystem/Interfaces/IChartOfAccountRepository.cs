using MiniAccountApi.Models;

namespace AccountManagementSystem.Repositories
{
    public interface IChartOfAccountRepository
    {
        Task<IEnumerable<ChartOfAccount>> GetRootAccountsAsync();
        Task<ChartOfAccount?> GetByIdAsync(int id);
        Task AddAsync(ChartOfAccount account);
        Task UpdateAsync(ChartOfAccount account);
        Task DeleteAsync(int id);
    }
}
