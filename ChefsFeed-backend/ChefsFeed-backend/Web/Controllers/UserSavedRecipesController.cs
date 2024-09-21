using ChefsFeed_backend.Services.Implementation;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [HttpGet("recipeByCategory")]
    public async Task<IActionResult> GetSavedRecipesByCategory(long categoryId)
    {
        var userId = GetUserId(); 
        var recipes = await _userSavedRecipesService.GetSavedRecipesByCategoryAsync(userId, categoryId, HttpContext);

        //if (recipes == null || !recipes.Any())
        //{
        //    return NotFound("No saved recipes found for the specified category.");
        //}

        return Ok(recipes);
    }

    private long GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            throw new Exception("User not found or JWT token is invalid.");
        }
        return userId;
    }
}
