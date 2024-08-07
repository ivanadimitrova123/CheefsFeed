using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Data;
using ChefsFeed_backend.Repositories.Interfaces;

namespace ChefsFeed_backend.Repositories.Implementation
{
    public class PictureRepository : IPictureRepository
    {
        private readonly ApplicationDbContext _context;

        public PictureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Picture> GetByIdAsync(long id)
        {
            return await _context.Pictures.FindAsync(id);
        }

        public async Task AddAsync(Picture picture)
        {
            await _context.Pictures.AddAsync(picture);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Picture> FindAsync(long id)
        {
            return await _context.Pictures.FindAsync(id);
        }
    }
}
