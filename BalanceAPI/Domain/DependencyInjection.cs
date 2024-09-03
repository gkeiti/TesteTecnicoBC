using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services
                .AddSingleton<BalanceServices>()
                ;

            return services;
        }
    }
}
