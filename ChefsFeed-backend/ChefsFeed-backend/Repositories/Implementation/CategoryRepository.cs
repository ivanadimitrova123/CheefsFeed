namespace ChefsFeed_backend.Repositories.Implementation;
using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Data;
using ChefsFeed_backend.Repositories.Interfaces;
using System.Threading.Tasks;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddCategoryAsync(Category category)
    {
        await _context.Category.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(long id)
    {
        return await _context.Category.FindAsync(id);
    }
}
