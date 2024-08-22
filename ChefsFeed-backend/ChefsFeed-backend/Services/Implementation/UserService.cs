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
        private readonly ICommentRepository _commentRepository;

        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IHttpContextAccessor httpContextAccessor, ICommentRepository commentRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<object> GetUserProfileAsync(long userId)
        {
            var userProfile = await _userRepository.GetUserByIdAsync(userId);

            if (userProfile == null)
            {
                return null;
            }

            string userImage = null;
            if (userProfile.ProfilePictureId != null)
            {
                userImage = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{userProfile.ProfilePictureId}";
            }

            var recipeData = new List<object>();

            foreach (var recipe in userProfile.Recipes)
            {
                var commentsCount = await _commentRepository.GetCommentsForRecipeAsync(recipe.Id);

                recipeData.Add(new
                {
                    recipe.Id,
                    recipe.Name,
                    recipe.PictureId,
                    RecipeImage = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{recipe.PictureId}",
                    Comments = commentsCount.Count(),
                    recipe.Rating
                });
            }

            var userData = new
            {
                Id = userProfile.Id,
                Username = userProfile.Username,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                ImageId = userProfile.ProfilePictureId,
                UserImage = userImage,
                Recipes = recipeData,
                Following = userProfile.Following.Count,
                Followers = userProfile.Followers.Count
            };

            return userData;
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
            var existingUserByEmail = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUserByEmail != null)
            {
                throw new ArgumentException("A user with this email already exists.");
            }

            var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUserByUsername != null)
            {
                throw new ArgumentException("A user with this username already exists.");
            }

            user.Password = _passwordHasher.HashPassword(user, user.Password);
            await _userRepository.AddUserAsync(user);
        }


        public async Task<(string Token, UserDto User)> LoginUserAsync(LogInUserDto model)
        {
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(model.Username);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password) != PasswordVerificationResult.Success)
            {
                return (null, null);
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

            string img = user.ProfilePictureId != null
                ? $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{user.ProfilePictureId}"
                : "";

            var userDto = new UserDto(user.Id, user.Username, user.FirstName, user.LastName, user.Email, img, user.Role);

            return (tokenString, userDto);
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

    }

}
