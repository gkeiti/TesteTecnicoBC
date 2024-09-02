namespace Domain.Entities
{
    public class BalanceEntity 
    {
        public BalanceEntity()
        {
            CreatedDate = DateTime.Now;
            IsActive = true;
        }

        public DateOnly Date { get; set; }
        public decimal ConsolidatedBalance { get; set; }
        public decimal SumDebits { get; set; }
        public decimal SumCredits { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
