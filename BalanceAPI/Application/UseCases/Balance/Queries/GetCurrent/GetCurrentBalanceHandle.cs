using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Balance.Queries.GetCurrent
{
    public class GetCurrentBalanceHandle : IRequestHandler<GetCurrentBalanceQuery, GetCurrentBalanceResponse>
    {
        private readonly IRedisService _redisService;
        private readonly IServiceScopeFactory _servicesScopeFactory;

        public GetCurrentBalanceHandle(IRedisService redisService, IServiceScopeFactory servicesScopeFactory)
        {
            _redisService = redisService;
            _servicesScopeFactory = servicesScopeFactory;
        }

        async Task<GetCurrentBalanceResponse> IRequestHandler<GetCurrentBalanceQuery, GetCurrentBalanceResponse>.Handle(GetCurrentBalanceQuery request, CancellationToken cancellationToken)
        {
            Console.WriteLine("GetCurrentBalanceHandler");

            BalanceEntity? sqlResult = null;

            var redisResult = _redisService.GetCurrentBalance();
            Console.WriteLine($"Buscou no redis. Result: {redisResult}");

            if (redisResult is not null)
            {
                Console.WriteLine("Return redisResult is not null");
                return new GetCurrentBalanceResponse(redisResult, "Redis");
            }
            else
            {
                using (var scope = _servicesScopeFactory.CreateScope())
                {
                    var _balanceService = scope.ServiceProvider.GetRequiredService<BalanceServices>();
                    var _balanceRepository = scope.ServiceProvider.GetRequiredService<IBalanceRepository>();

                    Console.WriteLine("Redis vazio, recalculando");

                    var saved = await _balanceService.RecalculateCurrentDay();

                    Console.WriteLine($"Recalculado: {saved}");

                    sqlResult = await _balanceRepository.GetCurrentBalanceAsync();

                    Console.WriteLine($"Result sql: {sqlResult} ");

                    if (sqlResult is not null)
                        _redisService.SaveCurrentBalanceRedis(sqlResult);
                }
            }

            var result = new GetCurrentBalanceResponse(sqlResult!, "SQL Server");

            return result;
        }
    }
}
