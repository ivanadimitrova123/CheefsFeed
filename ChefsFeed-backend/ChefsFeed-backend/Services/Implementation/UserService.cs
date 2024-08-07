using ChefsFeed_backend.Data.Models.Dtos;
using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Repositories.Interfaces;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ChefsFeed_backend.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserProfileAsync(long userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new User
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                Role = user.Role,
                Recipes = user.Recipes,
                Following = user.Following,
                Followers = user.Followers
            };
        }

        public async Task<User> GetCurrentUserInfoAsync(long userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new User
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                Role = user.Role,
                Recipes = user.Recipes,
                Following = user.Following,
                Followers = user.Followers
            };
        }

        public async Task RegisterUserAsync(User user)
        {
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            await _userRepository.AddUserAsync(user);
        }

        public async Task<string> LoginUserAsync(LogInUserDto model)
        {
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(model.Username);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password) != PasswordVerificationResult.Success)
            {
                return null;
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VkVSfGYr8VSkxDRF8ftKCwZuqN1lLLxBZN7s20jS"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:44365/",
                audience: "https://localhost:44365/",
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        
      public async Task<IEnumerable<UserDto>> SearchUsersAsync(string text)
        {
            var users = await _userRepository.SearchUsersAsync(text);
            return users.Select(user => new UserDto(
                 user.Id,
                 user.Username,
                 user.FirstName,
                 user.LastName,
                 user.Email,
                 user.ProfilePictureId != null
                     ? $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{user.ProfilePictureId}"
                     : string.Empty,
                 user.Role
             ));
        }
        /*
        public async Task<UserDto> GetUserProfileAsync(long userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureId != null
                    ? $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{user.ProfilePictureId}"
                    : null,
                Recipes = user.Recipes.Select(recipe => new RecipeDto
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    PictureId = recipe.PictureId,
                    RecipeImage = recipe.PictureId != null
                        ? $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{recipe.PictureId}"
                        : null,
                    Rating = recipe.Rating,
                    CommentsCount = _context.Comments.Count(c => c.RecipeId == recipe.Id)
                }).ToList(),
                FollowingCount = user.Following.Count,
                FollowersCount = user.Followers.Count
            };
        }
     */
    }

}
