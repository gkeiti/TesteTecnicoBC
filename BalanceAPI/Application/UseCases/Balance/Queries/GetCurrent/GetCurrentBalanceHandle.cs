using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using MediatR;

namespace Application.UseCases.Balance.Queries.GetCurrent
{
    public class GetCurrentBalanceHandle : IRequestHandler<GetCurrentBalanceQuery, GetCurrentBalanceResponse>
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly BalanceServices _balanceServices;

        public GetCurrentBalanceHandle(IBalanceRepository balanceRepository, BalanceServices balanceServices)
        {
            _balanceRepository = balanceRepository;
            _balanceServices = balanceServices;
        }

        async Task<GetCurrentBalanceResponse> IRequestHandler<GetCurrentBalanceQuery, GetCurrentBalanceResponse>.Handle(GetCurrentBalanceQuery request, CancellationToken cancellationToken)
        {
            // TODO: Resgatar do Redis, se nao tiver, resgatar do banco
            string redisResult = null;
            BalanceEntity sqlResult = null;
            if (redisResult is not null)
            {
                return new GetCurrentBalanceResponse(null);
            }
            else
            {
                sqlResult = await _balanceRepository.GetCurrentBalanceAsync();
            }

            if (sqlResult is null) 
            {
                await _balanceServices.RecalculateCurrentDay();
            }

            var result = new GetCurrentBalanceResponse(sqlResult);


            return result;
        }
    }
}
