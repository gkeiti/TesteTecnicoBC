using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services
{
    public interface IBalanceServices
    {
        void RecalculateCurrentDay();
    }

    public class BalanceServices : IBalanceServices
    {
        private readonly IOperationRepository _operationsRepository;
        private readonly IBalanceRepository _balanceRepository;

        public BalanceServices(IOperationRepository operationsRepository, IBalanceRepository balanceRepository)
        {
            _operationsRepository = operationsRepository;
            _balanceRepository = balanceRepository;
        }

        public async void RecalculateCurrentDay()
        {
            var operations = await _operationsRepository.GetAllOperationsOfCurrentDayAsync();

            decimal balance = 0;

            decimal debitsSum = operations.Where(x => x.Type == CashFlowType.Debit).Sum(x => x.Value);
            decimal creditsSum = operations.Where(x => x.Type == CashFlowType.Credit).Sum(x => x.Value);

            balance = creditsSum - debitsSum;

            var consolidated = new BalanceEntity
            {
                ConsolidatedBalance = balance,
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            await _balanceRepository.AddConsolidatedBalanceAsync(consolidated);
        }
    }
}
