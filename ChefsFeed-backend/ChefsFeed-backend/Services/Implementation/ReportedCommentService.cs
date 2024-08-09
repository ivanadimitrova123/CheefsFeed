namespace ChefsFeed_backend.Services.Implementation;

using ChefsFeed_backend.Data;
using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ReportedCommentService : IReportedCommentService
{
    private readonly ApplicationDbContext _context;

    public ReportedCommentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReportedComment>> GetReportedCommentsAsync()
    {
        return await _context.ReportedComments
            .Include(rc => rc.Comment)
            .Include(rc => rc.User)
            .ToListAsync();
    }

    public async Task<string> ReportCommentAsync(long userId, int commentId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return "User does not exist";
        }

        var commentExists = await _context.Comments.AnyAsync(c => c.CommentId == commentId);
        if (!commentExists)
        {
            return "Comment does not exist";
        }

        var alreadyReported = await _context.ReportedComments
            .AnyAsync(rc => rc.UserId == userId && rc.CommentId == commentId);

        if (alreadyReported)
        {
            return "Already reported";
        }

        var newReportedComment = new ReportedComment
        {
            UserId = userId,
            CommentId = commentId
        };

        await _context.ReportedComments.AddAsync(newReportedComment);
        await _context.SaveChangesAsync();

        return "Comment Reported";
    }

    public async Task<string> AllowReportedCommentAsync(int commentId)
    {
        var reportedComment = await _context.ReportedComments
            .FirstOrDefaultAsync(rc => rc.CommentId == commentId);

        if (reportedComment == null)
        {
            return "Reported comment does not exist";
        }

        _context.ReportedComments.Remove(reportedComment);
        await _context.SaveChangesAsync();

        return "Reported comment allowed";
    }

    public async Task<string> DeleteReportedCommentAsync(int commentId)
    {
        var reportedComment = await _context.ReportedComments
            .FirstOrDefaultAsync(rc => rc.CommentId == commentId);

        if (reportedComment == null)
        {
            return "Reported comment does not exist";
        }

        _context.ReportedComments.Remove(reportedComment);

        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.CommentId == commentId);

        if (comment != null)
        {
            _context.Comments.Remove(comment);
        }

        await _context.SaveChangesAsync();

        return "Comment deleted";
    }
}
