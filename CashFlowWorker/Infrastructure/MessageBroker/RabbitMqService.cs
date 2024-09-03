using Domain.Entities;
using Infrastructure.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Infrastructure.MessageBroker
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IServiceScopeFactory _servicesScopeFactory;

        public RabbitMqService(IServiceScopeFactory servicesScopeFactory)
        {
            _servicesScopeFactory = servicesScopeFactory;
        }

        public void Subscribe()
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq", Port = 5672, UserName = "guest", Password = "guest" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            string queueName = "operations-sent";
            string exchange = "operations-sent-exchange";

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);

            channel.QueueBind(queue: queueName,
                              exchange: exchange,
                              routingKey: string.Empty);

            Console.WriteLine(" [*] Waiting for operations.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] {message}");

                var operation = JsonConvert.DeserializeObject<OperationEntity>(message);

                using (var scope = _servicesScopeFactory.CreateScope())
                {
                    var _operationRepository = scope.ServiceProvider.GetRequiredService<IOperationRepository>();

                    var prevOperation = await _operationRepository.GetByGuid(operation!.Id);

                    if (prevOperation is null)
                    {
                        var saved = await _operationRepository.AddOperationAsync(operation);

                        if (saved)
                        {
                            Console.WriteLine("Teste aqui");
                            PublishSavedOperation(operation);
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                    }
                    else
                    {
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                }
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public void PublishSavedOperation(OperationEntity operation)
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq", Port = 5672, UserName = "guest", Password = "guest" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            string queueName = "operations-saved";
            string exchangeName = "operations-saved-exchange";

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: string.Empty);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var message = JsonConvert.SerializeObject(operation);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: exchangeName,
                                 routingKey: string.Empty,
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine($" [x] Sent {message}");
        }
    }
}
