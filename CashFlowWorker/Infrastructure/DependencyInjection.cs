using Infrastructure.Database.Contexts;
using Infrastructure.Database.Repositories;
using Infrastructure.MessageBroker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContexts(configuration)

                .AddSingleton<IRabbitMqService, RabbitMqService>()
                .AddScoped<IOperationRepository, OperationRepository>()
            ;

            return services;
        }

        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<CashFlowDbContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("BancoCarrefour")));
        }
    }
}
