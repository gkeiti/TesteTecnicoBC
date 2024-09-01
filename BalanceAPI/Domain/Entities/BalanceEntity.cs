namespace Domain.Entities
{
    public class BalanceEntity : EntityBase
    {
        public DateOnly Date { get; set; }
        public decimal SumDebits { get; set; }
        public decimal SumCredits { get; set; }
        public decimal ConsolidatedBalance { get; set; }
    }
}
