namespace ChefsFeed_backend.Web.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChefsFeed_backend.Services.Interfaces;

//upload  getId
[Route("api/image")]
[ApiController]
public class PictureController : ControllerBase
{
    private readonly IPictureService _pictureService;

    public PictureController(IPictureService pictureService)
    {
        _pictureService = pictureService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new { error = "User not authenticated" });
        }

        if (long.TryParse(userIdClaim.Value, out long userId))
        {
            try
            {
                var imageUrl = await _pictureService.UploadImageAsync(file, userId);
                return Ok(new { imageUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        return NotFound(new { error = "User not found or conversion failed" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(long id)
    {
        var imageData = await _pictureService.GetImageAsync(id);
        if (imageData == null)
        {
            return NotFound("Image not found");
        }

        return File(imageData, "image/jpeg"); // Use the appropriate content type here
    }
}