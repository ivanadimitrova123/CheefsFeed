using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(long userId);
        Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
        Task<IEnumerable<User>> SearchUsersAsync(string text);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByUsernameAsync(string username);

    }
}
