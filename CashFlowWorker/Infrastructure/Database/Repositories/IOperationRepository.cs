using Domain.Entities;

namespace Infrastructure.Database.Repositories
{
    public interface IOperationRepository
    {
        Task<bool> AddOperationAsync(OperationEntity operation);
    }
}