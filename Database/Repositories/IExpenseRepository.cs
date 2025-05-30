﻿using Database.Model;

namespace Database.Repositories
{
    public interface IExpenseRepository
    {
        void Add(TrackExpense expense);
        List<TrackExpense>? GetByUserId(string userId);
        List<TrackExpense>? GetByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate);
        void Update(TrackExpense expense);
        void Delete(TrackExpense expense);
    }
}