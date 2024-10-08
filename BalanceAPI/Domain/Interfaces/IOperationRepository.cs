﻿using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOperationRepository
    {
        Task<List<OperationEntity>> GetAllOperationsOfCurrentDayAsync();
    }
}