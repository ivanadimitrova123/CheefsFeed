using ChefsFeed_backend.Data.Models.Dtos;
using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserProfileAsync(long userId);
        Task<User> GetCurrentUserInfoAsync(long userId);
        Task RegisterUserAsync(User user);
        Task<string> LoginUserAsync(LogInUserDto model);
        Task<IEnumerable<UserDto>> SearchUsersAsync(string text);
        
    }
}
