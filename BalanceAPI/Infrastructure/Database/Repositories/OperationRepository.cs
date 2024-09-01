﻿using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class OperationRepository : IOperationRepository
    {
        public readonly BalanceDbContext _context;

        public OperationRepository(BalanceDbContext context)
        {
            _context = context;
        }

        public async Task<List<OperationEntity>> GetAllOperationsOfCurrentDayAsync()
        {
            return await _context.Operations.Where(x => x.CreatedDate.Date == DateTime.Now.Date).ToListAsync();
        }

        //public async Task<bool> AddOperationAsync(OperationEntity operation)
        //{
        //    await _context.Operations.AddAsync(operation);
        //    var result = await _context.SaveChangesAsync();

        //    return result == 1;
        //}
    }
}
