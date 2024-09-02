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
            var redisResult = await _balanceRepository.GetBalance();

            if (redisResult is not null)
            {
                return new GetCurrentBalanceResponse(redisResult!);
            }
            else
            {
                // TODO: Calcular saldo consolidado novamente
                _balanceServices.RecalculateCurrentDay();
                // TODO: Inserir no Redis
            }

            return null;
        }
    }
}
