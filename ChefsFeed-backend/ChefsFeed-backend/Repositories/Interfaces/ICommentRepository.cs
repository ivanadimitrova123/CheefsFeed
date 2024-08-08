using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> GetCommentByIdAsync(long id);
        Task<IEnumerable<Comment>> GetCommentsForRecipeAsync(long recipeId);
        Task AddCommentAsync(Comment comment);
        Task DeleteCommentAsync(long commentId);
    }
}
