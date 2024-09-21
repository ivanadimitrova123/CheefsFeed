using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Data;
using ChefsFeed_backend.Repositories.Interfaces;

namespace ChefsFeed_backend.Repositories.Implementation;

public class UserSavedRecipesRepository : IUserSavedRecipesRepository
{
    private readonly ApplicationDbContext _context;

    public UserSavedRecipesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByIdAsync(long userId)
    {
        return await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<Recipe> GetRecipeByIdAsync(long recipeId)
    {
        return await _context.Recipes.Include(r => r.User).SingleOrDefaultAsync(r => r.Id == recipeId);
    }

    public async Task<UserSavedRecipe> GetUserSavedRecipeAsync(long userId, long recipeId)
    {
        return await _context.UserSavedRecipe.SingleOrDefaultAsync(x => x.UserId == userId && x.RecipeId == recipeId);
    }

    public async Task SaveRecipeAsync(UserSavedRecipe userSavedRecipe)
    {
        _context.UserSavedRecipe.Add(userSavedRecipe);
        await _context.SaveChangesAsync();
    }

    public async Task<List<UserSavedRecipe>> GetSavedRecipesByUserIdAsync(long userId)
    {
        return await _context.UserSavedRecipe.Where(usr => usr.UserId == userId).ToListAsync();
    }
   
    public async Task<List<Recipe>> GetSavedRecipesByUserIdAndCategoryIdAsync(long userId, long categoryId)
    {
        var savedRecipes = await _context.UserSavedRecipe
            .Join(
                _context.Recipes,
                usr => usr.RecipeId,
                r => r.Id,
                (usr, r) => new { usr, r }
            )
            .Where(joined => joined.usr.UserId == userId 
            && joined.r.CategoryId == categoryId
             )
            .Select(joined => joined.r)
            .Distinct()
            .ToListAsync();

        return savedRecipes;
    }


    public int GetCommentsCountForRecipe(long recipeId)
    {
        return _context.Comments.Count(c => c.RecipeId == recipeId);
    }
    public async Task<int> GetCommentsCountForRecipeAsync(long recipeId)
    {
        return await _context.Comments.CountAsync(c => c.RecipeId == recipeId);
    }
}