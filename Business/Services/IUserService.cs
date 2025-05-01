using Database.Model;

namespace Business.Services
{
    public interface IUserService
    {
        void Register(User user, string password);
        string Login(string email, string password);
        User? GetUserById(string userId);
        void UpdateUser(User user);
        void DeleteUser(string userId);
    }
}