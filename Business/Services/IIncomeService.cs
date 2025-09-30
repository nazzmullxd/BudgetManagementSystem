using Database.Model;

namespace Business.Services
{
    public interface IIncomeService
    {
        void CreateIncome(TrackIncome income);
        List<TrackIncome>? GetIncomesByUserId(string userId);
        List<TrackIncome>? GetIncomesByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate);
        TrackIncome? GetIncomeById(string incomeId);
        void UpdateIncome(TrackIncome income);
        void DeleteIncome(string incomeId);
    }
}