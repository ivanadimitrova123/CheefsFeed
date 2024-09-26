namespace ChefsFeed_backend.Services.Implementation;

using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Repositories.Interfaces;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

public class ReportedRecipeService : IReportedRecipeService
{
    private readonly IReportedRecipeRepository _reportedRecipeRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReportedRecipeService(IReportedRecipeRepository reportedRecipeRepository, IRecipeRepository recipeRepository, IHttpContextAccessor httpContextAccessor)
    {
        _reportedRecipeRepository = reportedRecipeRepository;
        _recipeRepository = recipeRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<object>> GetReportedRecipesAsync()
    {
        var allReportedRecipes = await _reportedRecipeRepository.GetAllReportedRecipesAsync();
        var reportedRecipes = new List<object>();
        var recipeIds = new HashSet<long>();

        foreach (var recipe in allReportedRecipes)
        {
            if (recipeIds.Add(recipe.RecipeId))
            {
                var img = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{recipe.Recipe.PictureId}";
                reportedRecipes.Add(new
                {
                    recipe.RecipeId,
                    recipe.Recipe.Name,
                    img
                });
            }
        }

        return reportedRecipes;
    }

    public async Task<string> ReportRecipeAsync(long userId, long recipeId)
    {
        var userExists = await _reportedRecipeRepository.GetReportedRecipeAsync(userId, recipeId);
        if (userExists != null)
        {
            return "Already reported";
        }

        var reportedByOthers = await _reportedRecipeRepository.GetReportedRecipesByRecipeIdAsync(recipeId);
        if (reportedByOthers.Any())
        {
            return "Recipe Reported";
        }

        var newReportedRecipe = new ReportedRecipe { UserId = userId, RecipeId = recipeId };
        await _reportedRecipeRepository.AddReportedRecipeAsync(newReportedRecipe);

        return "Recipe Reported";
    }

    public async Task<bool> AllowReportedRecipeAsync(int recipeId)
    {
        var comment = await _recipeRepository.GetRecipeByIdAsync(recipeId);
        if (comment == null)
        {
            return false;
        }

        await _reportedRecipeRepository.RemoveReportedRecipeAsync(recipeId);

        return true;
    }

    public async Task<string> DeleteReportedRecipeAsync(long recipeId)
    {
        var reportedRecipe = _reportedRecipeRepository.GetReportedRecipesByRecipeIdAsync(recipeId);
        if (reportedRecipe == null)
        {
            return "Reported recipe does not exist";
        }
        await _reportedRecipeRepository.RemoveReportedRecipeAsync(recipeId);
        var recipe = _recipeRepository.GetRecipeByIdAsync(recipeId);
        if (recipe != null)
        {
            await _recipeRepository.DeleteRecipeAsync(recipeId);
        }

        return "Recipe deleted";

    }



}


