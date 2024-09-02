using Infrastructure.MessageBroker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Worker
{
    public class BalanceWorker : BackgroundService
    {
        private readonly ILogger<BalanceWorker> _logger;
        private readonly IRabbitMqService _rabbitMqService;

        public BalanceWorker(ILogger<BalanceWorker> logger, IRabbitMqService rabbitMqService)
        {
            _logger = logger;
            _rabbitMqService = rabbitMqService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _rabbitMqService.Subscribe();

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
