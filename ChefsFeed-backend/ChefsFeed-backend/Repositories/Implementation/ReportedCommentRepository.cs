namespace ChefsFeed_backend.Repositories.Implementation;
using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Data;
using ChefsFeed_backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ReportedCommentRepository : IReportedCommentRepository
{
    private readonly ApplicationDbContext _context;

    public ReportedCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReportedComment>> GetAllReportedCommentsAsync()
    {
        return await _context.ReportedComments.ToListAsync();
    }

    public async Task<ReportedComment> GetReportedCommentAsync(long userId, int commentId)
    {
        return await _context.ReportedComments
                             .FirstOrDefaultAsync(rc => rc.CommentId == commentId && rc.UserId == userId);
    }

    public async Task<IEnumerable<ReportedComment>> GetReportedCommentsByCommentIdAsync(int commentId)
    {
        return await _context.ReportedComments
                             .Where(rc => rc.CommentId == commentId)
                             .ToListAsync();
    }

    public async Task AddReportedCommentAsync(ReportedComment reportedComment)
    {
        await _context.ReportedComments.AddAsync(reportedComment);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveReportedCommentAsync(int commentId)
    {
        var reportedComment = await _context.ReportedComments
                                            .FirstOrDefaultAsync(rc => rc.CommentId == commentId);
        if (reportedComment != null)
        {
            _context.ReportedComments.Remove(reportedComment);
            await _context.SaveChangesAsync();
        }
    }
}
