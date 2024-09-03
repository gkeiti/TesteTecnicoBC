using Domain.Entities;
using Domain.Services;
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

            string queueName = "operations-saved";
            string exchange = "operations-saved-exchange";

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

                using (var scope = _servicesScopeFactory.CreateScope())
                {
                    var _balanceService = scope.ServiceProvider.GetRequiredService<BalanceServices>();
                    var saved = await _balanceService.RecalculateCurrentDay();

                    if (saved)
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
