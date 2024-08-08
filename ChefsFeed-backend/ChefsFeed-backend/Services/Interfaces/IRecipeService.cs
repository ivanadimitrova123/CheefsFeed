using ChefsFeed_backend.Data.Models.Dtos;
using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Services.Interfaces
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeDto>> GetPopularRecipesAsync();
        Task<IEnumerable<RecipeDto>> GetRecipesByUserIdAsync(long userId);
        Task<Recipe> GetRecipeByIdAsync(long id);
        Task<IEnumerable<SearchRecipeDto>> SearchRecipesAsync(string text);
        Task CreateRecipeAsync(Recipe recipe, long id, byte[] photoData, string photoContentType, List<long> selectedCategoryIds);
        Task UpdateRecipeAsync(long id, Recipe updatedRecipe, byte[] photoData, string photoContentType, List<long> selectedCategoryIds);
        Task DeleteRecipeAsync(long id);
    }
}
