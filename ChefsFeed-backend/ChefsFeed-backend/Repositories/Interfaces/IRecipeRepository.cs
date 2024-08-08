using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetPopularRecipesAsync();
        Task<IEnumerable<Recipe>> GetRecipesByUserIdAsync(long userId);
        Task<Recipe> GetRecipeByIdAsync(long id);
        Task<IEnumerable<Recipe>> SearchRecipesAsync(string text);
        Task AddRecipeAsync(Recipe recipe);
        Task UpdateRecipeAsync(Recipe recipe);
        Task DeleteRecipeAsync(long id);
    }
}
