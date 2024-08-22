namespace ChefsFeed_backend.Services.Implementation;

using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Repositories.Interfaces;
using ChefsFeed_backend.Services.Interfaces;
public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CommentService(ICommentRepository commentRepository, IHttpContextAccessor httpContextAccessor)
    {
        _commentRepository = commentRepository;
        _httpContextAccessor = httpContextAccessor;

    }

    public async Task<IEnumerable<object>> GetCommentsForRecipeAsync(long recipeId)
    {
        var comments = await _commentRepository.GetCommentsForRecipeAsync(recipeId);

        if (comments == null)
        {
            return Enumerable.Empty<object>();
        }

        var editedComments = comments.Select(comment => new
        {
            comment.CommentId,
            comment.ParentId,
            Username = comment.User?.Username,
            UserImage = comment.User?.ProfilePictureId != null
                ? $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{comment.User.ProfilePictureId}"
                : null,
            comment.Content,
            Children = comment.Children.Select(child => new
            {
                child.CommentId,
                child.Content
            })
        }).ToList();

        return editedComments;
    }

    public async Task CreateCommentAsync(Comment comment)
    {
        await _commentRepository.AddCommentAsync(comment);
    }

    public async Task DeleteCommentAsync(long commentId)
    {
        await _commentRepository.DeleteCommentAsync(commentId);
    }
}

