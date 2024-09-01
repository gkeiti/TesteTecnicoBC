using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly BalanceDbContext _balanceDbContext;

        public BalanceRepository(BalanceDbContext balanceDbContext)
        {
            _balanceDbContext = balanceDbContext;
        }

        public async Task<BalanceEntity?> GetBalance()
        {
            var balance = await _balanceDbContext.Balances.Where(x => x.Date == DateOnly.FromDateTime(DateTime.Now))
                                                    .FirstOrDefaultAsync();

            return balance;
        }

        public async Task AddConsolidatedBalanceAsync(BalanceEntity balance)
        {
            _balanceDbContext.Balances.Add(balance);
            await _balanceDbContext.SaveChangesAsync();
        }
    }
}
