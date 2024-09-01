using Domain.Entities;

namespace Application.UseCases.Balance.Queries.GetCurrent
{
    public class GetCurrentBalanceResponse
    {
        public GetCurrentBalanceResponse(BalanceEntity balance)
        {
            Date = balance.Date;
            CurrentBalance = balance.ConsolidatedBalance;
        }

        public DateOnly Date { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
