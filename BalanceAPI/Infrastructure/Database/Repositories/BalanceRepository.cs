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

        public async Task<BalanceEntity?> GetCurrentBalanceAsync()
        {
            var balance = await _balanceDbContext.Balances.Where(x => x.Date == DateOnly.FromDateTime(DateTime.Now))
                                                    .FirstOrDefaultAsync();

            return balance;
        }

        public async Task<int> AddConsolidatedBalanceAsync(BalanceEntity balance)
        {
            _balanceDbContext.Balances.Add(balance);
            return await _balanceDbContext.SaveChangesAsync();
        }

        public async Task<int> AddOrUpdateBalanceCurrentDayAsync(BalanceEntity balance)
        {
            var currBalance = await _balanceDbContext.Balances.AsNoTracking().FirstOrDefaultAsync(x => x.Date == balance.Date);

            if (currBalance is null)
                await _balanceDbContext.Balances.AddAsync(balance);
            else
            {
                balance.UpdatedDate = DateTime.Now;
                _balanceDbContext.Balances.Update(balance);
            }

            return await _balanceDbContext.SaveChangesAsync();
        }
    }
}
