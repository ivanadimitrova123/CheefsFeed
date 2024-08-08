using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChefsFeed_backend.Web.Controllers;

[Route("api/usergrades")]
[ApiController]
public class UserGradesController : ControllerBase
{
    private readonly IUserGradesService _userGradesService;

    public UserGradesController(IUserGradesService userGradesService)
    {
        _userGradesService = userGradesService;
    }

    [HttpPost]
    public async Task<IActionResult> GradeRecipe([FromForm] long userId, [FromForm] long recipeId, [FromForm] int grade)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized("You must be logged in to grade a recipe.");
        }

        return await _userGradesService.GradeRecipeAsync(userId, recipeId, grade);
    }

    [HttpGet]
    public async Task<IActionResult> HasGradedRecipe([FromQuery] long userId, [FromQuery] long recipeId)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized("You must be logged in to check if you have graded a recipe.");
        }

        return await _userGradesService.HasGradedRecipeAsync(userId, recipeId);
    }
}
