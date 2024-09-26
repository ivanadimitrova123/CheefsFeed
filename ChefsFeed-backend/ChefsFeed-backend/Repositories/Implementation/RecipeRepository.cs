using ChefsFeed_backend.Data;
using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Repositories.Interfaces;

namespace ChefsFeed_backend.Repositories.Implementation
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationDbContext _context;

        public RecipeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipe>> GetPopularRecipesAsync()
        {
            return await _context.Recipes
                .Include(r => r.User)
                .OrderByDescending(r => _context.Comments.Count(c => c.RecipeId == r.Id))  
                .Take(7)  
                .ToListAsync();
        }


        public async Task<IEnumerable<Recipe>> GetRecipesByUserIdAsync(long userId)
        {
            return await _context.Recipes.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Recipe>> GetRecipesByCategoryAsync(long categoryId, long excludeRecipeId)
        {
            return await _context.Recipes
                .Where(r => r.CategoryId == categoryId && r.Id != excludeRecipeId)
                .ToListAsync();
        }


        public async Task<Recipe> GetRecipeByIdAsync(long id)
        {
            return await _context.Recipes
                .Include(r => r.Picture)
                .Include(r => r.User)
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Recipe>> SearchRecipesAsync(string text)
        {
            return await _context.Recipes.Include(r => r.Picture)
                .Where(r => r.Name.Contains(text))
                .Take(3)
                .ToListAsync();
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecipeAsync(long id)
        {
            var recipe = await _context.Recipes
                              // .Include(r => r.Categories)
                               .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe != null)
            {
                /*if (recipe.Categories.Any())
                {
                    _context.Category.RemoveRange(recipe.Categories);
                }*/
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
        }
    }
}