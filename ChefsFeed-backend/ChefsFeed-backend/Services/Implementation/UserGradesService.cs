using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Repositories.Interfaces;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChefsFeed_backend.Services.Implementation;

public class UserGradesService : IUserGradesService
{
    private readonly IUserRepository _userRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUserGradesRepository _userGradesRepository;

    public UserGradesService(IUserRepository userRepository, IRecipeRepository recipeRepository, IUserGradesRepository userGradesRepository)
    {
        _userRepository = userRepository;
        _recipeRepository = recipeRepository;
        _userGradesRepository = userGradesRepository;
    }

    public async Task<IActionResult> GradeRecipeAsync(long userId, long recipeId, int grade)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            return new BadRequestObjectResult("No user with that id");
        }

        var recipe = await _recipeRepository.GetRecipeByIdAsync(recipeId);
        if (recipe == null)
        {
            return new BadRequestObjectResult("No recipe with that id");
        }

        var userGrade = await _userGradesRepository.GetUserGradeAsync(userId, recipeId);
        if (userGrade == null)
        {
            userGrade = new UserGrades
            {
                UserId = userId,
                RecipeId = recipeId,
                Grade = grade
            };
            await _userGradesRepository.AddUserGradeAsync(userGrade);
        }
        else
        {
            userGrade.Grade = grade;
            await _userGradesRepository.UpdateUserGradeAsync(userGrade);
        }

        var grades = await _userGradesRepository.GetUserGradesByRecipeIdAsync(recipeId);
        var averageGrade = grades.Average(g => g.Grade);
        recipe.Rating = (int)Math.Round(averageGrade);
        await _recipeRepository.UpdateRecipeAsync(recipe);

        return new OkResult();
    }

    public async Task<IActionResult> HasGradedRecipeAsync(long userId, long recipeId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            return new BadRequestObjectResult("No user with that id");
        }

        var recipe = await _recipeRepository.GetRecipeByIdAsync(recipeId);
        if (recipe == null)
        {
            return new BadRequestObjectResult("No recipe with that id");
        }

        var userGrade = await _userGradesRepository.GetUserGradeAsync(userId, recipeId);
        var reviews = await _userGradesRepository.GetUserGradesByRecipeIdAsync(recipeId);
        var reviewCount = reviews.Count();

        if (userGrade == null)
        {
            return new OkObjectResult(new { Grade = 0, Reviews = reviewCount });
        }

        return new OkObjectResult(new { userGrade.Grade, Reviews = reviewCount });
    }
}

