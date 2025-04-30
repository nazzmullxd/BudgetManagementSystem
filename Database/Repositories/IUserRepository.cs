using Database.Model;

namespace Database.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetById(string userId);
        User? GetByEmail(string email);
        void Update(User user);
        void Delete(User user);
    }
}