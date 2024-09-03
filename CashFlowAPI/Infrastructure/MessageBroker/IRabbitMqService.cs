namespace Infrastructure.MessageBroker
{
    public interface IRabbitMqService
    {
        void PublishOperation<T>(T payload);
    }
}