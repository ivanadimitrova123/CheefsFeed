namespace ChefsFeed_backend.Services.Interfaces;

using ChefsFeed_backend.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IReportedCommentService
{
    Task<IEnumerable<ReportedComment>> GetReportedCommentsAsync();
    Task<string> ReportCommentAsync(long userId, int commentId);
    Task<string> AllowReportedCommentAsync(int commentId);
    Task<string> DeleteReportedCommentAsync(int commentId);
}
