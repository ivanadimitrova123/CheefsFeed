using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Data;
using ChefsFeed_backend.Repositories.Interfaces;

namespace ChefsFeed_backend.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(long userId)
        {
            return await _context.Users
                .Include(u => u.Following)
                .Include(u => u.Followers)
                .Include(u => u.Recipes)
                .Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await SaveChangesAsync();
        }

        
          public async Task<IEnumerable<User>> SearchUsersAsync(string text)
        {
            return await _context.Users
                .Where(u => u.Username.Contains(text))
                .Take(3)
                .ToListAsync();
        }
        
         
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

    }
}
