using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Repositories.Implementation;
using ChefsFeed_backend.Repositories.Interfaces;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChefsFeed_backend.Services.Implementation
{
    public class UserSavedRecipesService : IUserSavedRecipesService
    {
        private readonly IUserSavedRecipesRepository _repository;
        private readonly IUserRepository _userrepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSavedRecipesService(IUserSavedRecipesRepository repository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _userrepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> SaveRecipeAsync(long recipeId, long userId, HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return new UnauthorizedResult();
            }

            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new BadRequestObjectResult("No user with that id");
            }

            var recipe = await _repository.GetRecipeByIdAsync(recipeId);
            if (recipe == null)
            {
                return new BadRequestObjectResult("No recipe with that id");
            }

            var existingSavedRecipe = await _repository.GetUserSavedRecipeAsync(userId, recipeId);
            if (existingSavedRecipe != null)
            {
                return new OkObjectResult("Already Saved");
            }

            var saveRecipe = new UserSavedRecipe
            {
                UserId = userId,
                RecipeId = recipeId
            };

            await _repository.SaveRecipeAsync(saveRecipe);
            return new OkResult();
        }

        public async Task<IActionResult> GetSavedRecipesAsync(long userId, HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return new UnauthorizedResult();
            }

            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new BadRequestObjectResult("No user with that id");
            }

            var savedRecipes = await _repository.GetSavedRecipesByUserIdAsync(userId);
            var recipesList = new List<object>();

            foreach (var savedRecipe in savedRecipes)
            {
                var recipe = await _repository.GetRecipeByIdAsync(savedRecipe.RecipeId);
                if (recipe != null && recipe.User != null)
                {
                    var userImage = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/image/{recipe.User.ProfilePictureId}";
                    var commentsCount = _repository.GetCommentsCountForRecipe(recipe.Id);
                    recipesList.Add(new
                    {
                        recipe = new
                        {
                            recipe.Id,
                            recipe.Name,
                            recipe.PictureId,
                            RecipeImage = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/image/{recipe.PictureId}",
                            Comments = commentsCount,
                            recipe.Rating
                        },
                        user = new { userImage, recipe.User.Username }
                    });
                }
            }

            return new OkObjectResult(recipesList);
        }

        //public async Task<List<Recipe>> GetSavedRecipesByCategoryAsync(long userId, long categoryId)
        //{
        //    return await _repository.GetSavedRecipesByUserIdAndCategoryIdAsync(userId, categoryId);
        //}

        //public async Task<List<dynamic>> GetSavedRecipesByCategoryAsync(long userId, long categoryId)
        //{
        //    var savedRecipes = await _repository.GetSavedRecipesByUserIdAndCategoryIdAsync(userId, categoryId);

        //    if (savedRecipes == null || !savedRecipes.Any())
        //    {
        //        return new List<dynamic>();
        //    }
        //   // User u =await _userrepository.GetUserByIdAsync(Recipe.UserId);

        //    var recipeDetails = savedRecipes.Select(recipe => new
        //    {
        //        //UserImage = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{u.ProfilePictureId}",
        //        //Username = u.Username,
        //        RecipeId = recipe.Id,
        //        RecipeName = recipe.Name,
        //        RecipeImage = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{recipe.PictureId}",
        //        Rating = recipe.Rating,
        //        CommentCount = _repository.GetCommentsCountForRecipe(recipe.Id)
        //    }).ToList<dynamic>();

        //    return recipeDetails;
        //}
        public async Task<List<dynamic>> GetSavedRecipesByCategoryAsync(long userId, long categoryId)
        {
            var savedRecipes = await _repository.GetSavedRecipesByUserIdAndCategoryIdAsync(userId, categoryId);

            if (savedRecipes == null || !savedRecipes.Any())
            {
                return new List<dynamic>();
            }

            var httpContext = _httpContextAccessor.HttpContext;
            var requestScheme = httpContext?.Request.Scheme ?? "http";
            var requestHost = httpContext?.Request.Host.ToString() ?? "localhost";

            // Create a list of tasks to fetch user details
            var tasks = savedRecipes.Select(async recipe =>
            {
                // Fetch the user for the recipe
                var user = await _userrepository.GetUserByIdAsync(recipe.UserId);

                return new
                {
                    UserImage = user != null
                        ? $"{requestScheme}://{requestHost}/api/image/{user.ProfilePictureId}"
                        : $"{requestScheme}://{requestHost}/api/image/default-profile.png",
                    Username = user?.Username ?? "Unknown",
                    RecipeId = recipe.Id,
                    RecipeName = recipe.Name,
                    RecipeImage = recipe.PictureId.HasValue
                        ? $"{requestScheme}://{requestHost}/api/image/{recipe.PictureId.Value}"
                        : $"{requestScheme}://{requestHost}/api/image/default-recipe.png",
                    Rating = recipe.Rating,
                    CommentCount =  _repository.GetCommentsCountForRecipe(recipe.Id) 
                };
            }).ToList();

            var recipeDetails = await Task.WhenAll(tasks);

            return recipeDetails.ToList<dynamic>();
        }


    }
}
