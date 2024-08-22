using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<object>> GetCommentsForRecipeAsync(long recipeId);
        Task CreateCommentAsync(Comment comment);
        Task DeleteCommentAsync(long commentId);
    }
}
