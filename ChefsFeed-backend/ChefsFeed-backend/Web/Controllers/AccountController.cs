namespace ChefsFeed_backend.Web.Controllers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ChefsFeed_backend.Data.Models;
using Microsoft.IdentityModel.Tokens;
using ChefsFeed_backend.Data.Models.Dtos;
using ChefsFeed_backend.Data;
using ChefsFeed_backend.Services.Interfaces;

//get +getID +search getCurrent register login

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("allUsers")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
    
    [HttpGet("search")]
       public async Task<IActionResult> SearchUser([FromQuery] string text)
       {
           var userDtos = await _userService.SearchUsersAsync(text);
           return Ok(userDtos);
       }
    /*
       [HttpGet("user/{userId}")]
       public async Task<IActionResult> GetUserProfile(long userId)
       {
           var userDto = await _userService.GetUserProfileAsync(userId);
           if (userDto == null)
           {
               return NotFound("User not found.");
           }

           return Ok(userDto);
       }
    */
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            return NotFound("User not found");
        }

        var userData = await _userService.GetCurrentUserInfoAsync(userId);
        if (userData == null)
        {
            return NotFound("User not found");
        }

        return Ok(userData);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _userService.RegisterUserAsync(model);
            return Ok("Registration successful.");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Registration", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LogInUserDto model)
    {
        var token = await _userService.LoginUserAsync(model);

        return Ok(token);
    }
}