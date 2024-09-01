using MediatR;

namespace Application.UseCases.Debit.Commands.AddDebitCommand
{
    public class AddDebitCommand : IRequest
    {
        public decimal Value { get; set; }
        public string? Description { get; set; }
    }
}
