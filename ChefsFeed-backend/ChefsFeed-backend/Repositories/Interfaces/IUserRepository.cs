using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
        Task<IEnumerable<User>> SearchUsersAsync(string text);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(long userId);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();

    }
}
