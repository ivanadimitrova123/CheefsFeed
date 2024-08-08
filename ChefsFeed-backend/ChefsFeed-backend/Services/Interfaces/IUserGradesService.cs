using Microsoft.AspNetCore.Mvc;

namespace ChefsFeed_backend.Services.Interfaces;

public interface IUserGradesService
{
    Task<IActionResult> GradeRecipeAsync(long userId, long recipeId, int grade);
    Task<IActionResult> HasGradedRecipeAsync(long userId, long recipeId);
}
