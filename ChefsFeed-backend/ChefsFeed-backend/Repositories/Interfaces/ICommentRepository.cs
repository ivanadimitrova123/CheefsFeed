using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsForRecipeAsync(long recipeId);
        Task<Comment> GetCommentByIdAsync(long id);
        Task DeleteCommentAsync(long commentId);
        Task AddCommentAsync(Comment comment);
    }
}
