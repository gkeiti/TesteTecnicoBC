using Domain.Entities;

namespace Application.UseCases.Balance.Queries.GetCurrent
{
    public class GetCurrentBalanceResponse
    {
        public GetCurrentBalanceResponse(BalanceEntity balance, string source)
        {
            Date = balance.Date;
            CurrentBalance = balance.ConsolidatedBalance;
            Source = source;
        }

        public DateOnly Date { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Source { get; set; }
    }
}
