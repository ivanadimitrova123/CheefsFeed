using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces;

public interface IUserSavedRecipesRepository
{
    Task<List<Recipe>> GetSavedRecipesByUserIdAndCategoryIdAsync(long userId, long categoryId);
    Task<UserSavedRecipe> GetUserSavedRecipeAsync(long userId, long recipeId);
    Task<List<UserSavedRecipe>> GetSavedRecipesByUserIdAsync(long userId);
    Task SaveRecipeAsync(UserSavedRecipe userSavedRecipe);
    Task<Recipe> GetRecipeByIdAsync(long recipeId);
    int GetCommentsCountForRecipe(long recipeId);
    Task<User> GetUserByIdAsync(long userId);
}
