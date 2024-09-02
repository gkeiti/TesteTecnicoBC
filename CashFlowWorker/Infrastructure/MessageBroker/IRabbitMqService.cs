
using Domain.Entities;

namespace Infrastructure.MessageBroker
{
    public interface IRabbitMqService
    {
        void Subscribe();
        void PublishSavedOperation(OperationEntity operation);
    }
}