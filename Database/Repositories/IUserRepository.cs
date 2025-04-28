using Database.Model;  // For User

namespace BudgetManagementSystem.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        User GetById(string userId);
        User GetByEmail(string email);
        void Update(User user);
    }
}