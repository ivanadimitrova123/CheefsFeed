﻿namespace ChefsFeed_backend.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/recipes")]
[Authorize]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
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
    public async Task<IActionResult> CreateRecipe([FromForm] Recipe recipe, IFormFile photo, [FromForm] List<long> selectedCategoryIds)
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

        await _recipeService.CreateRecipeAsync(recipe, userId, photoData, photoContentType, selectedCategoryIds);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditRecipe(long id, [FromForm] Recipe updatedRecipe, IFormFile? photo, [FromForm] List<long> selectedCategoryIds)
    {
        var userId = GetUserIdFromClaims();

        // Optionally: Check if user is authorized to edit (e.g., Admin or owner)
        // ...

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
            await _recipeService.UpdateRecipeAsync(id, updatedRecipe, photoData, photoContentType, selectedCategoryIds);
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

        // Optionally: Check if user is authorized to delete (e.g., Admin )

        try
        {
            await _recipeService.DeleteRecipeAsync(id);
            return NoContent();
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
}
