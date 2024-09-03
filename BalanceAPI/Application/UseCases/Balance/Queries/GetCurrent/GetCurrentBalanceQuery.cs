using MediatR;

namespace Application.UseCases.Balance.Queries.GetCurrent
{
    public class GetCurrentBalanceQuery : IRequest<GetCurrentBalanceResponse>
    {
    }
}
