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

        public UserSavedRecipesService(IUserSavedRecipesRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userrepository = userRepository;
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

        /* public async Task<List<dynamic>> GetSavedRecipesByCategoryAsync(long userId, long categoryId, HttpContext httpContext)
         {
             var savedRecipes = await _repository.GetSavedRecipesByUserIdAndCategoryIdAsync(userId, categoryId);

             if (savedRecipes == null || !savedRecipes.Any())
             {
                 return new List<dynamic>();
             }

             var requestScheme = httpContext.Request.Scheme;
             var requestHost = httpContext.Request.Host.ToString();

             var tasks = savedRecipes.Select(async recipe =>
             {
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
         }*/


        public async Task<List<dynamic>> GetSavedRecipesByCategoryAsync(long userId, long categoryId, HttpContext httpContext)
        {
            var savedRecipes = await _repository.GetSavedRecipesByUserIdAndCategoryIdAsync(userId, categoryId);

            // Return an empty list if no saved recipes are found
            if (savedRecipes == null || !savedRecipes.Any())
            {
                return new List<dynamic>();
            }

            var requestScheme = httpContext.Request.Scheme;
            var requestHost = httpContext.Request.Host.ToString();

            var tasks = savedRecipes.Select(async recipe =>
            {
                var user = await _userrepository.GetUserByIdAsync(recipe.UserId);

                return new
                {
                    recipe = new
                    {
                        Id = recipe.Id,
                        Name = recipe.Name,
                        PictureId = recipe.PictureId,
                        RecipeImage = recipe.PictureId.HasValue
                            ? $"{requestScheme}://{requestHost}/api/image/{recipe.PictureId.Value}"
                            : $"{requestScheme}://{requestHost}/api/image/default-recipe.png",
                        Rating = recipe.Rating,
                        Comments = _repository.GetCommentsCountForRecipe(recipe.Id) 
                    },
                    user = new
                    {
                        UserImage = user != null
                            ? $"{requestScheme}://{requestHost}/api/image/{user.ProfilePictureId}"
                            : $"{requestScheme}://{requestHost}/api/image/default-profile.png",
                        Username = user?.Username ?? "Unknown"
                    }
                };
            }).ToList();

            var recipeDetails = await Task.WhenAll(tasks);

            return recipeDetails.ToList<dynamic>();
        }


    }
}
