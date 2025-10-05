using Business.Models;
using Database.Model;

namespace Business.Services
{
    public interface IUserService
    {
        void Register(User user, string password);
        Task<LoginResponse> LoginAsync(string email, string password);
        User? GetUserById(string userId);
        void UpdateUser(User user);
        void DeleteUser(string userId);
        void ChangePassword(string userId, string currentPassword, string newPassword);
        bool ValidatePasswordStrength(string password, out List<string> validationErrors);
        Task<LoginResponse> RefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}