using MediatR;

namespace Application.UseCases.Credit.Commands.AddCreditCommand
{
    public class AddCreditCommand : IRequest
    {
        public decimal Value { get; set; }
        public string? Description { get; set; }
    }
}
