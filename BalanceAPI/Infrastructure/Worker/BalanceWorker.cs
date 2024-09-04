using Infrastructure.MessageBroker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Worker
{
    public class BalanceWorker : BackgroundService
    {
        private readonly IRabbitMqService _rabbitMqService;

        public BalanceWorker(IRabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(10000, stoppingToken);
            _rabbitMqService.Subscribe();
        }
    }
}
