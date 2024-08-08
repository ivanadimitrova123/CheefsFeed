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

        public async Task<IEnumerable<RecipeDto>> GetPopularRecipesAsync()
        {
            var recipes = await _recipeRepository.GetPopularRecipesAsync();
            return recipes.Select(recipe => new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                RecipeImage = $"http://example.com/api/image/{recipe.PictureId}"
            });
        }

        public async Task<IEnumerable<RecipeDto>> GetRecipesByUserIdAsync(long userId)
        {
            var recipes = await _recipeRepository.GetRecipesByUserIdAsync(userId);
            return recipes.Select(recipe => new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                RecipeImage = $"http://example.com/api/image/{recipe.PictureId}"
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
            return recipes.Select(recipe => new SearchRecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Picture = $"http://example.com/api/image/{recipe.PictureId}"
            });
        }

        public async Task CreateRecipeAsync(Recipe recipe, long id, byte[] photoData, string photoContentType, List<long> selectedCategoryIds)
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

            foreach (var categoryId in selectedCategoryIds)
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
                if (category != null)
                {
                    recipe.Categories.Add(category);
                }
            }

            await _recipeRepository.AddRecipeAsync(recipe);
        }

        public async Task UpdateRecipeAsync(long id, Recipe updatedRecipe, byte[] photoData, string photoContentType, List<long> selectedCategoryIds)
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

            recipe.Name = updatedRecipe.Name;
            recipe.Description = updatedRecipe.Description;
            recipe.Ingredients = updatedRecipe.Ingredients;
            recipe.Level = updatedRecipe.Level;
            recipe.Cook = updatedRecipe.Cook;
            recipe.Prep = updatedRecipe.Prep;
            recipe.Total = updatedRecipe.Total;
            recipe.Yield = updatedRecipe.Yield;

            recipe.Categories.Clear();
            foreach (var categoryId in selectedCategoryIds)
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
                if (category != null)
                {
                    recipe.Categories.Add(category);
                }
            }

            await _recipeRepository.UpdateRecipeAsync(recipe);
        }

        public async Task DeleteRecipeAsync(long id)
        {
            await _recipeRepository.DeleteRecipeAsync(id);
        }
    }
}
