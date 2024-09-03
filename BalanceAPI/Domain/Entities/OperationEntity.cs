using Domain.Enums;

namespace Domain.Entities
{
    public class OperationEntity : EntityBase
    {
        public OperationEntity(decimal value, string? description, CashFlowType type)
        {
            Value = value;
            Description = description;
            Type = type;
        }

        public decimal Value { get; set; }
        public string? Description { get; set; }
        public CashFlowType Type { get; set; }
    }
}
