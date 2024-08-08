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

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<IEnumerable<Comment>> GetCommentsForRecipeAsync(long recipeId)
    {
        var comments = await _commentRepository.GetCommentsForRecipeAsync(recipeId);
        if (comments == null)
        {
            return Enumerable.Empty<Comment>();
        }
        return comments.Select(c => new Comment
        {
        CommentId = c.CommentId,
        ParentId = c.ParentId,
        UserId = c.User?.Id ?? 0, // Use null conditional operator and default value if User is null
        User = c.User != null ? new User
        {
            Id = c.User.Id,
            Username = c.User.Username,
            ProfilePictureId = c.User.ProfilePictureId
        } : null,
        RecipeId = c.Recipe?.Id ?? 0, // Use null conditional operator and default value if Recipe is null
        Recipe = c.Recipe != null ? new Recipe
        {
            Id = c.Recipe.Id,
            Name = c.Recipe.Name,
            PictureId = c.Recipe.PictureId
        } : null,
        Content = c.Content,
        Parent = c.Parent != null ? new Comment
        {
            CommentId = c.Parent.CommentId,
            Content = c.Parent.Content
            // You might want to include more fields from the parent if needed
        } : null,
        Children = c.Children != null ? c.Children.Select(child => new Comment
        {
            CommentId = child.CommentId,
            Content = child.Content
            // You might want to include more fields from the child if needed
        }).ToList() : new List<Comment>()
    });
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

