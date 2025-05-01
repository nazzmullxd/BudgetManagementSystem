using Database.Model;

namespace Database.Repositories
{
    public interface IIncomeRepository
    {
        void Add(TrackIncome income);
        List<TrackIncome>? GetByUserId(string userId);
        List<TrackIncome>? GetByUserIdAndDateRange(string userId, DateTime startDate, DateTime endDate);
        void Update(TrackIncome income);
        void Delete(TrackIncome income);
    }
}