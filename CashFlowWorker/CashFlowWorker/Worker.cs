using Infrastructure.MessageBroker;

namespace CashFlowWorker
{
    public class Worker : BackgroundService
    {
        private readonly IRabbitMqService _rabbitMqService;

        public Worker(IRabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _rabbitMqService.Subscribe();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
