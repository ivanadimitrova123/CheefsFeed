namespace ChefsFeed_backend.Web.Controllers;
using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/comments")]
[ApiController]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentsForRecipe(long id)
    {
        var comments = await _commentService.GetCommentsForRecipeAsync(id);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment([FromForm] Comment model)
    {
        if (ModelState.IsValid)
        {
            await _commentService.CreateCommentAsync(model);
            return Ok();
        }
        return BadRequest("Error with creating comment");
    }

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> DeleteComment(long commentId)
    {
        await _commentService.DeleteCommentAsync(commentId);
        return Ok();
    }
}
