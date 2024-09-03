using Domain.Entities;
using Infrastructure.MessageBroker;
using MediatR;

namespace Application.UseCases.Credit.Commands.AddCreditCommand
{
    public class AddCreditHandler : IRequestHandler<AddCreditCommand>
    {
        private readonly IRabbitMqService _rabbitMqService;

        public AddCreditHandler(IRabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        public Task Handle(AddCreditCommand request, CancellationToken cancellationToken)
        {
            var teste = new OperationEntity(request.Value, request.Description, Domain.Enums.CashFlowType.Credit);

            _rabbitMqService.PublishOperation<OperationEntity>(teste);

            return Task.CompletedTask;
        }
    }
}
