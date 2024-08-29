using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefsFeed_backend.Web.Controllers
{
    [Route("api/reportedrecipe")]
    [ApiController]
    public class ReportedRecipeController : ControllerBase
    {
        private readonly IReportedRecipeService _reportedRecipeService;

        public ReportedRecipeController(IReportedRecipeService reportedRecipeService)
        {
            _reportedRecipeService = reportedRecipeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportedRecipes()
        {
            var reportedRecipes = await _reportedRecipeService.GetReportedRecipesAsync();
            return Ok(reportedRecipes);
        }

        [HttpPost]
        public async Task<IActionResult> ReportRecipe([FromForm] long userId, [FromForm] long recipeId)
        {
            var result = await _reportedRecipeService.ReportRecipeAsync(userId, recipeId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{recipeId}")]
        public async Task<IActionResult> DeleteReportedRecipe(long recipeId)
        {
            var result = await _reportedRecipeService.DeleteReportedRecipeAsync(recipeId);
            return Ok(result);
        }
    }
}
