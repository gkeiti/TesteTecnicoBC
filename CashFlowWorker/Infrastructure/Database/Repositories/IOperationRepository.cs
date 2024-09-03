using Domain.Entities;

namespace Infrastructure.Database.Repositories
{
    public interface IOperationRepository
    {
        Task<OperationEntity?> GetByGuid(Guid guid);
        Task<bool> AddOperationAsync(OperationEntity operation);
    }
}