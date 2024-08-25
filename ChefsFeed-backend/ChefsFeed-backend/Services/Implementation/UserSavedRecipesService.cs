using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Repositories.Interfaces;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChefsFeed_backend.Services.Implementation
{
    public class UserSavedRecipesService : IUserSavedRecipesService
    {
        private readonly IUserSavedRecipesRepository _repository;

        public UserSavedRecipesService(IUserSavedRecipesRepository repository)
        {
            _repository = repository;
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
                    var commentsCount = await _repository.GetCommentsCountForRecipeAsync(recipe.Id);
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
    }
}
