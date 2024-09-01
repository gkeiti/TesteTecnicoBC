using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBalanceRepository
    {
        Task<BalanceEntity?> GetBalance();
        Task AddConsolidatedBalanceAsync(BalanceEntity balance);
    }
}