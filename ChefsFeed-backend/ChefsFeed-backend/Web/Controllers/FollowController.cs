﻿namespace ChefsFeed_backend.Web.Controllers;
using System.Security.Claims;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/follow")]
[ApiController]
public class FollowController : ControllerBase
{
    private readonly IFollowService _followService;

    public FollowController(IFollowService followService)
    {
        _followService = followService;
    }

    [HttpGet("status/{followedUserId}")]
    public IActionResult IsFollowed(long followedUserId)
    {
        var userId = GetUserId();
        var result = _followService.IsFollowed(userId, followedUserId);
        return Ok(result);
    }

    [HttpPost("{followedUserId}")]
    public IActionResult FollowUser(long followedUserId)
    {
        var userId = GetUserId();
        var result = _followService.FollowUser(userId, followedUserId);
        if (result == "User not found." || result == "User to be followed not found.")
        {
            return NotFound(result);
        }
        if (result == "You cannot follow yourself." || result == "You are already following this user.")
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpDelete("{followedUserId}")]
    public IActionResult UnfollowUser(long followedUserId)
    {
        var userId = GetUserId();
        var result = _followService.UnfollowUser(userId, followedUserId);
        if (result == "User not found." || result == "User to be unfollowed not found.")
        {
            return NotFound(result);
        }
        if (result == "You are not currently following this user.")
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet("following")]
    public IActionResult GetFollowingUsers()
    {
        var userId = GetUserId();
        var users = _followService.GetFollowingUsers(userId, HttpContext);
        if (users == null)
        {
            return NotFound("User not found.");
        }
        return Ok(users);
    }

    [HttpGet("recipes")]
    public IActionResult GetRecipesOfFollowedUsers()
    {
        var userId = GetUserId();

        // Get the request scheme and host from HttpContext
        var requestScheme = HttpContext.Request.Scheme;
        var requestHost = HttpContext.Request.Host.ToString();

        var recipesWithImages = _followService.GetRecipesOfFollowedUsers(userId, requestScheme, requestHost);

        if (recipesWithImages == null)
        {
            return NotFound("User not found.");
        }

        return Ok(recipesWithImages);
    }


    private long GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            throw new Exception("User not found or JWT token is invalid.");
        }
        return userId;
    }
}

