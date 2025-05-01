using Database.Repositories;

namespace Business.Services
{
    public abstract class BaseService
    {
        protected readonly IUserRepository _userRepository;

        protected BaseService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected void ValidateUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID is required.");
            }

            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }
        }

        protected void ValidateDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be later than end date.");
            }
        }
    }
}