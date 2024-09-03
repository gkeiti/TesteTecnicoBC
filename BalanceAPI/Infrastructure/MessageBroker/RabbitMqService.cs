using Domain.Entities;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace Infrastructure.MessageBroker
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IServiceScopeFactory _servicesScopeFactory;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqService(IServiceScopeFactory servicesScopeFactory)
        {
            _servicesScopeFactory = servicesScopeFactory;

            // Localhost
            //var factory = new ConnectionFactory { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };

            // Docker
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true
            };

            Console.WriteLine("Tentando conectar");

            _connection = factory.CreateConnection();
            Console.WriteLine("Connection criada");

            _channel = _connection.CreateModel();
            Console.WriteLine("Model criado");
        }

        public void Subscribe()
        {
            try
            {
                string queueName = "operations-saved";
                string exchange = "operations-saved-exchange";

                _channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                Console.WriteLine("Queue declarada");

                _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);
                Console.WriteLine("Exchange declarada");

                _channel.QueueBind(queue: queueName,
                                  exchange: exchange,
                                  routingKey: string.Empty);
                Console.WriteLine("Queue bindada");


                Console.WriteLine(" [*] Waiting for operations.");

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (model, ea) =>
                {
                    byte[] body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" Mensagem lida: {message}");

                    using (var scope = _servicesScopeFactory.CreateScope())
                    {
                        var _balanceService = scope.ServiceProvider.GetRequiredService<BalanceServices>();
                        var saved = await _balanceService.RecalculateCurrentDay();
                        Console.WriteLine("Mensagem salva");


                        if (saved)
                        {
                            Console.WriteLine("Mensagem acknoledged");
                            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                    }
                };
                _channel.BasicConsume(queue: queueName,
                                     autoAck: false,
                                     consumer: consumer);
                Console.WriteLine("Basic Consume configurado");

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro Rabbit: {ex.ToString()}");
            }
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
