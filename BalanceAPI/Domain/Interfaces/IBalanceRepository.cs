using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBalanceRepository
    {
        Task<BalanceEntity?> GetCurrentBalanceAsync();
        Task<int> AddConsolidatedBalanceAsync(BalanceEntity balance);
        Task<int> AddOrUpdateBalanceCurrentDayAsync(BalanceEntity balance);
    }
}