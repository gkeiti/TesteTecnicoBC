using Domain.Interfaces;
using Infrastructure.CacheDatabase.Service;
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
                .AddSingleton<IRedisService, RedisService>()
                .AddScoped<IOperationRepository, OperationRepository>()
                .AddScoped<IBalanceRepository, BalanceRepository>()
                .AddScoped<IRedisService, RedisService>()
            ;


            return services;
        }

        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = configuration.GetConnectionString("Redis");
                    options.InstanceName = "Redis";
                })
                .AddDbContext<BalanceDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("BancoCarrefour"));
                })
            ;
        }
    }
}
