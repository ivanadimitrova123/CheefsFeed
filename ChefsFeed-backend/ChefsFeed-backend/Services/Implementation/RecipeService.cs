using ChefsFeed_backend.Data.Models.Dtos;
using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Repositories.Interfaces;
using ChefsFeed_backend.Services.Interfaces;

namespace ChefsFeed_backend.Services.Implementation
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecipeService(IRecipeRepository recipeRepository, IPictureRepository pictureRepository, ICategoryRepository categoryRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _recipeRepository = recipeRepository;
            _pictureRepository = pictureRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<object>> GetPopularRecipesAsync()
        {
            var recipes = await _recipeRepository.GetPopularRecipesAsync();
            var editedRecipes = new List<object>();

            foreach (var recipe in recipes)
            {
                string imgUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{recipe.PictureId}";

                editedRecipes.Add(new
                {
                    recipe.Id,
                    recipe.Name,
                    img = imgUrl,
                    recipe.Total,
                    recipe.Level,
                    Username = recipe.User?.Username
                });
            }

            return editedRecipes;
        }

        public async Task<IEnumerable<object>> GetRecommendedRecipesAsync(long recipeId)
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(recipeId);

            if (recipe == null || recipe.CategoryId == null)
            {
                return new List<object>();
            }

            var recommendedRecipes = await _recipeRepository.GetRecipesByCategoryAsync((long)recipe.CategoryId, recipeId);
            var editedRecipes = new List<object>();

            foreach (var rec in recommendedRecipes)
            {
                string imgUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{rec.PictureId}";

                editedRecipes.Add(new
                {
                    rec.Id,
                    rec.Name,
                    img = imgUrl,
                    rec.Total,
                    rec.Level,
                    Username = rec.User?.Username
                });
            }

            return editedRecipes;
        }


        public async Task<IEnumerable<RecipeDto>> GetRecipesByUserIdAsync(long userId)
        {
            var recipes = await _recipeRepository.GetRecipesByUserIdAsync(userId);
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/";
            return recipes.Select(recipe => new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                RecipeImage = $"{baseUrl}{recipe.PictureId}"
            });
        }

        public async Task<Recipe> GetRecipeByIdAsync(long id)
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(id);
            if (recipe == null) return null;

            var ingredients = recipe.Ingredients[0].Replace("\r", "").Split("\n").ToList();
            recipe.Ingredients = ingredients;

            return new Recipe
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                PictureId = recipe.PictureId,
                Picture=recipe.Picture,
                Ingredients = recipe.Ingredients,
                UserId= recipe.UserId,
                User = recipe.User,
                CategoryId = recipe.CategoryId,
                Level = recipe.Level,
                Cook = recipe.Cook,
                Prep = recipe.Prep,
                Total = recipe.Total,
                Yield = recipe.Yield,
                Rating = recipe.Rating,
            };
        }

        public async Task<IEnumerable<SearchRecipeDto>> SearchRecipesAsync(string text)
        {
            var recipes = await _recipeRepository.SearchRecipesAsync(text);
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/";
            return recipes.Select(recipe => new SearchRecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Picture = recipe.PictureId != null ? $"{baseUrl}{recipe.PictureId}" : string.Empty
            });
        }

        public async Task CreateRecipeAsync(Recipe recipe, long id, byte[] photoData, string photoContentType, long categoryId)
        {

            if (photoData != null)
            {
                var picture = new Picture
                {
                    ImageData = photoData,
                    ContentType = photoContentType,
                    FileName = ""
                };
                await _pictureRepository.AddAsync(picture);
                await _pictureRepository.SaveChangesAsync();
                picture.FileName = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/image/{picture.Id}";
                recipe.Picture = picture;
                recipe.PictureId = picture.Id;

                User u = await _userRepository.GetUserByIdAsync(id);
                recipe.UserId = id;
                recipe.User = u;
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category != null)
            {
                recipe.Category = category;
            }

            await _recipeRepository.AddRecipeAsync(recipe);
        }

        public async Task UpdateRecipeAsync(long id, Recipe updatedRecipe, byte[] photoData, string photoContentType, long categoryId)
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(id);
            if (recipe == null) throw new KeyNotFoundException("Recipe not found");

            if (photoData != null)
            {
                var picture = new Picture
                {
                    ImageData = photoData,
                    ContentType = photoContentType,
                    FileName = ""
                };
                await _pictureRepository.AddAsync(picture);
                recipe.Picture = picture;
                recipe.PictureId = picture.Id;
            }

            recipe.Description = updatedRecipe.Description;
            recipe.Ingredients = updatedRecipe.Ingredients;
            recipe.Name = updatedRecipe.Name + "_Whatever_ID";
            recipe.Level = updatedRecipe.Level;
            recipe.Cook = updatedRecipe.Cook;
            recipe.Prep = updatedRecipe.Prep;
            recipe.Total = updatedRecipe.Total;
            recipe.Yield = updatedRecipe.Yield;

            //recipe.Categories.Clear();
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category != null)
            {
                recipe.Category = category; 
            }
            else
            {
                throw new KeyNotFoundException("Category not found");
            }

            await _recipeRepository.UpdateRecipeAsync(recipe);
        }

        public async Task DeleteRecipeAsync(long id)
        {
            await _recipeRepository.DeleteRecipeAsync(id);
        }
    }
}
