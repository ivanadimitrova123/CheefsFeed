namespace ChefsFeed_backend.Services.Interfaces;
public interface IReportedRecipeService
{
    Task<string> ReportRecipeAsync(long userId, long recipeId);
    Task<string> DeleteReportedRecipeAsync(long recipeId);
    Task<IEnumerable<object>> GetReportedRecipesAsync();
    
}