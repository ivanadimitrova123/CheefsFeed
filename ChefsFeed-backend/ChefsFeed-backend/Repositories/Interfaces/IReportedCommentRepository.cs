using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces;

public interface IReportedCommentRepository
{
    Task<IEnumerable<ReportedComment>> GetAllReportedCommentsAsync();
    Task<ReportedComment> GetReportedCommentAsync(long userId, int commentId);
    Task<IEnumerable<ReportedComment>> GetReportedCommentsByCommentIdAsync(int commentId);
    Task AddReportedCommentAsync(ReportedComment reportedComment);
    Task RemoveReportedCommentAsync(int commentId);
}