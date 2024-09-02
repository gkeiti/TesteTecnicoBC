using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Services
{
    public interface IBalanceServices
    {
        Task<bool> RecalculateCurrentDay();
    }

    public class BalanceServices : IBalanceServices
    {
        private readonly IServiceScopeFactory _servicesScopeFactory;

        public BalanceServices(IServiceScopeFactory servicesScopeFactory)
        {
            _servicesScopeFactory = servicesScopeFactory;
        }

        public async Task<bool> RecalculateCurrentDay()
        {
            using (var scope = _servicesScopeFactory.CreateScope())
            {
                var _operationRepository = scope.ServiceProvider.GetRequiredService<IOperationRepository>();
                var _balanceRepository = scope.ServiceProvider.GetRequiredService<IBalanceRepository>();

                var operations = await _operationRepository.GetAllOperationsOfCurrentDayAsync();

                decimal balance = 0;

                decimal debitsSum = operations.Where(x => x.Type == CashFlowType.Debit).Sum(x => x.Value);
                decimal creditsSum = operations.Where(x => x.Type == CashFlowType.Credit).Sum(x => x.Value);

                balance = creditsSum - debitsSum;

                var consolidated = new BalanceEntity
                {
                    ConsolidatedBalance = balance,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    SumDebits = debitsSum,
                    SumCredits = creditsSum
                };

                var saved = await _balanceRepository.AddOrUpdateBalanceCurrentDayAsync(consolidated);

                if (saved > 0)
                {
                    var _redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();

                    _redisService.SaveCurrentBalanceRedis(consolidated);
                }

                return saved > 0;
            }
        }
    }
}
