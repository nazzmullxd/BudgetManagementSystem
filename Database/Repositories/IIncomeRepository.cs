using Database.Model;
using System;
using System.Collections.Generic;

namespace BudgetManagementSystem.Repositories
{
    public interface IIncomeRepository
    {
        void Add(TrackIncome income);
        List<TrackIncome> GetByUserId(string userId);
        List<TrackIncome> GetByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate);
    }
}