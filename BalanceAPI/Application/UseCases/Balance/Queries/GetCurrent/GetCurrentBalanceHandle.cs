using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Balance.Queries.GetCurrent
{
    public class GetCurrentBalanceHandle : IRequestHandler<GetCurrentBalanceQuery, GetCurrentBalanceResponse>
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IRedisService _redisService;

        public GetCurrentBalanceHandle(IBalanceRepository balanceRepository, IRedisService redisService)
        {
            _balanceRepository = balanceRepository;
            _redisService = redisService;
        }

        async Task<GetCurrentBalanceResponse> IRequestHandler<GetCurrentBalanceQuery, GetCurrentBalanceResponse>.Handle(GetCurrentBalanceQuery request, CancellationToken cancellationToken)
        {
            BalanceEntity? sqlResult = null;

            var redisResult = _redisService.GetCurrentBalance();

            if (redisResult is not null)
            {
                return new GetCurrentBalanceResponse(redisResult);
            }
            else
            {
                sqlResult = await _balanceRepository.GetCurrentBalanceAsync();

                if (sqlResult is not null)
                    _redisService.SaveCurrentBalanceRedis(sqlResult);
            }

            var result = new GetCurrentBalanceResponse(sqlResult!);


            return result;
        }
    }
}
