namespace ChefsFeed_backend.Services.Implementation;

using ChefsFeed_backend.Data;
using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Repositories.Interfaces;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ReportedCommentService : IReportedCommentService
{
    private readonly IReportedCommentRepository _reportedCommentRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICommentRepository _commentRepository;
    public ReportedCommentService(IReportedCommentRepository reportedCommentRepository, IUserRepository userRepository, ICommentRepository commentRepository)
    {
        _reportedCommentRepository = reportedCommentRepository;
        _userRepository = userRepository;
        _commentRepository = commentRepository; 
    }

    public async Task<IEnumerable<ReportedComment>> GetReportedCommentsAsync()
    {
        return await _reportedCommentRepository.GetAllReportedCommentsAsync();
    }

    public async Task<string> ReportCommentAsync(long userId, int commentId)
    {
        var user = _userRepository.GetUserByIdAsync(userId);

        var comment = _commentRepository.GetCommentByIdAsync(commentId);

        var alreadyReported =await _reportedCommentRepository.GetReportedCommentAsync(userId, commentId);
      
        if (alreadyReported != null)
        {
            return "Already reported";
        }

        var newReportedComment = new ReportedComment
        {
            UserId = userId,
            CommentId = commentId
        };

        await _reportedCommentRepository.AddReportedCommentAsync(newReportedComment);

        return "Comment Reported";
    }

    public async Task<bool> AllowReportedCommentAsync(int commentId)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);
        if (comment == null)
        {
            return false;
        }

        await _reportedCommentRepository.RemoveReportedCommentAsync(commentId);

        return true;
    }


    public async Task<string> DeleteReportedCommentAsync(int commentId)
    {
        var reportedComment = await _reportedCommentRepository.GetReportedCommentsByCommentIdAsync(commentId);

        if (reportedComment == null)
        {
            return "Reported comment does not exist";
        }
        await _reportedCommentRepository.RemoveReportedCommentAsync(commentId);

        var comment = _commentRepository.GetCommentByIdAsync(commentId);

        if (comment != null)
        {
            await _commentRepository.DeleteCommentAsync(commentId);
        }
        return "Comment deleted";
    }
}
