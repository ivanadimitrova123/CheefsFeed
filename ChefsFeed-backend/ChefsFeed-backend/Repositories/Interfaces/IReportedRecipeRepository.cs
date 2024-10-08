﻿using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces;

public interface IReportedRecipeRepository
{
    Task<IEnumerable<ReportedRecipe>> GetReportedRecipesByRecipeIdAsync(long recipeId);
    Task<ReportedRecipe> GetReportedRecipeAsync(long userId, long recipeId);
    Task<IEnumerable<ReportedRecipe>> GetAllReportedRecipesAsync();
    Task AddReportedRecipeAsync(ReportedRecipe reportedRecipe);
    Task RemoveReportedRecipeAsync(long recipeId);
}