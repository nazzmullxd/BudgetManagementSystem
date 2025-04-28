using Database.Model;
using System;
using System.Collections.Generic;

namespace BudgetManagementSystem.Repositories
{
    public interface IExpenseRepository
    {
        void Add(TrackExpense expense);
        List<TrackExpense> GetByUserId(string userId);
        List<TrackExpense> GetByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate);
    }
}