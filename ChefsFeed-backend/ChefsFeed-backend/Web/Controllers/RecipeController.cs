namespace ChefsFeed_backend.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ChefsFeed_backend.Repositories.Interfaces;

[ApiController]
[Route("api/recipes")]
[Authorize]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUserRepository _userRepository;


    public RecipeController(IRecipeService recipeService, IUserRepository userRepository, IRecipeRepository recipeRepository)
    {
        _recipeService = recipeService;
        _userRepository = userRepository;
        _recipeRepository = recipeRepository;
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularRecipes()
    {
        var recipes = await _recipeService.GetPopularRecipesAsync();
        return Ok(recipes);
    }

    [HttpGet]
    public async Task<IActionResult> GetRecipes()
    {
        var userId = GetUserIdFromClaims();
        var recipes = await _recipeService.GetRecipesByUserIdAsync(userId);
        return Ok(recipes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipeById(long id)
    {
        var recipe = await _recipeService.GetRecipeByIdAsync(id);
        if (recipe == null)
        {
            return NotFound("Recipe not found");
        }
        return Ok(recipe);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchRecipe([FromQuery] string text)
    {
        var recipes = await _recipeService.SearchRecipesAsync(text);
        return Ok(recipes);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecipe([FromForm] Recipe recipe, IFormFile photo, [FromForm] long categoryId)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized("You must be logged in to create a recipe.");
        }

        var userId = GetUserIdFromClaims();
        byte[] photoData = null;
        string photoContentType = null;

        if (photo != null && photo.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);
                photoData = memoryStream.ToArray();
                photoContentType = photo.ContentType;
            }
        }

        await _recipeService.CreateRecipeAsync(recipe, userId, photoData, photoContentType, categoryId);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditRecipe(long id, [FromForm] Recipe updatedRecipe, IFormFile? photo, [FromForm] long categoryId)
    {
        var userId = GetUserIdFromClaims();

        var existingRecipe = await _recipeRepository.GetRecipeByIdAsync(id);
        if (existingRecipe == null)
        {
            return NotFound("Recipe not found");
        }

        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user.Role != "Admin" && existingRecipe.UserId != userId)
        {
            return Forbid("You are not authorized to edit this recipe.");
        }

        byte[] photoData = null;
        string photoContentType = null;

        if (photo != null && photo.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);
                photoData = memoryStream.ToArray();
                photoContentType = photo.ContentType;
            }
        }

        try
        {
            await _recipeService.UpdateRecipeAsync(id, updatedRecipe, photoData, photoContentType, categoryId);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Recipe not found");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(long id)
    {
        var userId = GetUserIdFromClaims();
        var currentUserRole = GetUserRoleFromClaims();
        try
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);

            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }
            if (currentUserRole == "Admin" || recipe.UserId == userId)
            {
                await _recipeService.DeleteRecipeAsync(id);
                return NoContent();
            }

            return Forbid("You do not have permission to delete this recipe.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Recipe not found");
        }
    }

    private long GetUserIdFromClaims()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            throw new UnauthorizedAccessException("User not found or conversion failed.");
        }
        return userId;
    }
    private string GetUserRoleFromClaims()
    {
        return User.FindFirst(ClaimTypes.Role)?.Value ?? "User";
    }
}
