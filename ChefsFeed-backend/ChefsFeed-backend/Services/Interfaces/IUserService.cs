using ChefsFeed_backend.Data.Models.Dtos;
using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<(string Token, UserDto User)> LoginUserAsync(LogInUserDto model);
        Task<IEnumerable<UserDto>> SearchUsersAsync(string text);
        Task<User> GetCurrentUserInfoAsync(long userId);
        Task<object> GetUserProfileAsync(long userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task RegisterUserAsync(User user);
        
    }
}
