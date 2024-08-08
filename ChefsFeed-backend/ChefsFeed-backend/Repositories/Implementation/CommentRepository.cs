namespace ChefsFeed_backend.Repositories.Implementation;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Data;
using ChefsFeed_backend.Repositories.Interfaces;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> GetCommentByIdAsync(long id)
    {
        return await _context.Comments
            .Include(c => c.User)
            .Include(c => c.Children)
            .SingleOrDefaultAsync(c => c.CommentId == id);
    }

    public async Task<IEnumerable<Comment>> GetCommentsForRecipeAsync(long recipeId)
    {
        return await _context.Comments
            .Where(c => c.RecipeId == recipeId)
            .Include(c => c.Children)
            .Include(c => c.User)
            .ToListAsync();
    }

    public async Task AddCommentAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(long commentId)
    {
        var comments = await _context.Comments
            .Include(c => c.Children)
            .Where(c => c.CommentId == commentId)
            .ToListAsync();

        var flatten = Flatten(comments);
        _context.Comments.RemoveRange(flatten);
        await _context.SaveChangesAsync();
    }

    private IEnumerable<Comment> Flatten(IEnumerable<Comment> comments) =>
        comments.SelectMany(c => Flatten(c.Children)).Concat(comments);
}
