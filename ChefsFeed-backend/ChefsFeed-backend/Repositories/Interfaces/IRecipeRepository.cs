using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetRecipesByUserIdAsync(long userId);
        Task<IEnumerable<Recipe>> SearchRecipesAsync(string text);
        Task<IEnumerable<Recipe>> GetPopularRecipesAsync();
        Task<IEnumerable<Recipe>> GetRecipesByCategoryAsync(long categoryId, long excludeRecipeId);
        Task<Recipe> GetRecipeByIdAsync(long id);
        Task UpdateRecipeAsync(Recipe recipe);
        Task AddRecipeAsync(Recipe recipe);
        Task DeleteRecipeAsync(long id);
    }
}
