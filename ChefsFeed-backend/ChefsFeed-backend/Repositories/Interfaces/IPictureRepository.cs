using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces
{
    public interface IPictureRepository
    {
        Task<Picture> GetByIdAsync(long id);
        Task AddAsync(Picture picture);
        Task SaveChangesAsync();
        Task<Picture> FindAsync(long id);
    }
}
