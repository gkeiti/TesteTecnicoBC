using Domain.Entities;
using Infrastructure.Database.Contexts;

namespace Infrastructure.Database.Repositories
{
    public class OperationRepository : IOperationRepository
    {
        public readonly CashFlowDbContext _context;

        public OperationRepository(CashFlowDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOperationAsync(OperationEntity operation)
        {
            await _context.Operations.AddAsync(operation);
            var result = await _context.SaveChangesAsync();

            return result == 1;
        }
    }
}
