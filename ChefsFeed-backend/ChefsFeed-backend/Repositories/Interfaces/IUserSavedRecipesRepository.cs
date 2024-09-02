using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces;

public interface IUserSavedRecipesRepository
{
    Task<User> GetUserByIdAsync(long userId);
    Task<Recipe> GetRecipeByIdAsync(long recipeId);
    Task<UserSavedRecipe> GetUserSavedRecipeAsync(long userId, long recipeId);
    Task SaveRecipeAsync(UserSavedRecipe userSavedRecipe);
    Task<List<UserSavedRecipe>> GetSavedRecipesByUserIdAsync(long userId);
    Task<List<Recipe>> GetSavedRecipesByUserIdAndCategoryIdAsync(long userId, long categoryId);
    int GetCommentsCountForRecipe(long recipeId);
}
