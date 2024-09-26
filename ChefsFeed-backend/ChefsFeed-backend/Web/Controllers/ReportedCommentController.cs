using ChefsFeed_backend.Services.Implementation;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefsFeed_backend.Web.Controllers;

[Route("api/reportcomment")]
[ApiController]
public class ReportedCommentController : ControllerBase
{
    private readonly IReportedCommentService _reportedCommentService;

    public ReportedCommentController(IReportedCommentService reportedCommentService)
    {
        _reportedCommentService = reportedCommentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetReportedComments()
    {
        var reportedComments = await _reportedCommentService.GetReportedCommentsAsync();
        var result = new List<object>();

        foreach (var rc in reportedComments)
        {
            var img = rc.User.ProfilePictureId != null
                ? $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/image/{rc.User.ProfilePictureId}"
                : "";

            result.Add(new
            {
                rc.CommentId,
                Comment = rc.Comment,
                User = new
                {
                    rc.User.Id,
                    rc.User.Username,
                    rc.User.FirstName,
                    rc.User.LastName,
                    rc.User.Email,
                    img,
                    rc.User.Role
                }
            });
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> ReportComment([FromForm] long userId, [FromForm] int commentId)
    {
        var result = await _reportedCommentService.ReportCommentAsync(userId, commentId);
        return Ok(result);
    }

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> AllowReportedComment(int commentId)
    {
        var result = await _reportedCommentService.AllowReportedCommentAsync(commentId);
        if (!result)
        {
            return BadRequest("Comment does not exist");
        }

        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("delete/{commentId}")]
    public async Task<IActionResult> DeleteReportedComment(int commentId)
    {
        var result = await _reportedCommentService.DeleteReportedCommentAsync(commentId);
        return Ok(result);
    }
}