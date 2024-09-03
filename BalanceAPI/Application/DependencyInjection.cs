using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assembly));

            services
                .AddScoped<IBalanceRepository, BalanceRepository>()
                .AddTransient<BalanceServices>()
                ;

            return services;
        }
    }
}
