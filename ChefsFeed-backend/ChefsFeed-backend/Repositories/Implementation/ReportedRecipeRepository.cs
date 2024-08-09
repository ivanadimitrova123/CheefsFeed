namespace ChefsFeed_backend.Repositories.Implementation;

using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Data;
using ChefsFeed_backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ReportedRecipeRepository : IReportedRecipeRepository
{
    private readonly ApplicationDbContext _context;

    public ReportedRecipeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReportedRecipe>> GetAllReportedRecipesAsync()
    {
        return await _context.ReportedRecipes.Include(r => r.Recipe).ToListAsync();
    }

    public async Task<ReportedRecipe> GetReportedRecipeAsync(long userId, long recipeId)
    {
        return await _context.ReportedRecipes
                .FirstOrDefaultAsync(rr => rr.RecipeId == recipeId && rr.UserId == userId);
    }

    public async Task<IEnumerable<ReportedRecipe>> GetReportedRecipesByRecipeIdAsync(long recipeId)
    {
        return await _context.ReportedRecipes
                             .Where(rr => rr.RecipeId == recipeId)
                             .ToListAsync();
    }

    public async Task AddReportedRecipeAsync(ReportedRecipe reportedRecipe)
    {
        await _context.ReportedRecipes.AddAsync(reportedRecipe);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveReportedRecipeAsync(long recipeId)
    {
        var reportedRecipe = await _context.ReportedRecipes
                            .FirstOrDefaultAsync(rr => rr.RecipeId == recipeId);
        if (reportedRecipe != null)
        {
            _context.ReportedRecipes.Remove(reportedRecipe);
            await _context.SaveChangesAsync();
        }
    }
}
