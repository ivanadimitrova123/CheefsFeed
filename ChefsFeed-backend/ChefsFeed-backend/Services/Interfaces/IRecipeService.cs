using ChefsFeed_backend.Data.Models.Dtos;
using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Services.Interfaces
{
    public interface IRecipeService
    {
        Task UpdateRecipeAsync(long id, Recipe updatedRecipe, byte[] photoData, string photoContentType, long selectedCategoryIds);
        Task CreateRecipeAsync(Recipe recipe, long id, byte[] photoData, string photoContentType, long selectedCategoryIds);
        Task<IEnumerable<SearchRecipeDto>> SearchRecipesAsync(string text);
        Task<IEnumerable<RecipeDto>> GetRecipesByUserIdAsync(long userId);
        Task<IEnumerable<object>> GetPopularRecipesAsync();
        Task<Recipe> GetRecipeByIdAsync(long id);
        Task DeleteRecipeAsync(long id);
        
    }
}
