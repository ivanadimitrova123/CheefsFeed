using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChefsFeed_backend.Web.Controllers;

[Route("api/saverecipe")]
[ApiController]
public class UserSavedRecipesController : ControllerBase
{
    private readonly IUserSavedRecipesService _userSavedRecipesService;

    public UserSavedRecipesController(IUserSavedRecipesService userSavedRecipesService)
    {
        _userSavedRecipesService = userSavedRecipesService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveRecipe([FromForm] long recipeId, [FromForm] long userId)
    {
        return await _userSavedRecipesService.SaveRecipeAsync(recipeId, userId, HttpContext);
    }

    [HttpGet]
    public async Task<IActionResult> GetSavedRecipes(long userId)
    {
        return await _userSavedRecipesService.GetSavedRecipesAsync(userId, HttpContext);
    }
}
