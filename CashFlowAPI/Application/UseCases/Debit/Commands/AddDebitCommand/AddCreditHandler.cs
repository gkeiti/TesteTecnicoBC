using Domain.Entities;
using Infrastructure.MessageBroker;
using MediatR;

namespace Application.UseCases.Debit.Commands.AddDebitCommand
{
    public class AddDebitHandler : IRequestHandler<AddDebitCommand>
    {
        private readonly IRabbitMqService _rabbitMqService;

        public AddDebitHandler(IRabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        public Task Handle(AddDebitCommand request, CancellationToken cancellationToken)
        {
            var teste = new OperationEntity(request.Value, request.Description, Domain.Enums.CashFlowType.Debit);

            _rabbitMqService.PublishOperation<OperationEntity>(teste);

            return Task.CompletedTask;
        }
    }
}
