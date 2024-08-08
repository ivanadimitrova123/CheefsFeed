using Microsoft.AspNetCore.Mvc;

namespace ChefsFeed_backend.Services.Interfaces;

public interface IUserSavedRecipesService
{
    Task<IActionResult> SaveRecipeAsync(long recipeId, long userId, HttpContext httpContext);
    Task<IActionResult> GetSavedRecipesAsync(long userId, HttpContext httpContext);
}