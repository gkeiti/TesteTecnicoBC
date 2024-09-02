using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Infrastructure.MessageBroker
{
    public class RabbitMqService : IRabbitMqService
    {
        public void PublishOperation<T>(T payload)
        {
            var factory = new ConnectionFactory { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "operations-sent",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.ExchangeDeclare(exchange: "operations", type: ExchangeType.Fanout); 
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var message = JsonConvert.SerializeObject(payload);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "operations",
                                 routingKey: string.Empty,
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine($" [x] Sent {message}");
        }
    }
}
