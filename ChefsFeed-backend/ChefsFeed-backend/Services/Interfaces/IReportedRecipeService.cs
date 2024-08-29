namespace ChefsFeed_backend.Services.Interfaces;
public interface IReportedRecipeService
{
    Task<IEnumerable<object>> GetReportedRecipesAsync();
    Task<string> ReportRecipeAsync(long userId, long recipeId);
    Task<string> DeleteReportedRecipeAsync(long recipeId);
}