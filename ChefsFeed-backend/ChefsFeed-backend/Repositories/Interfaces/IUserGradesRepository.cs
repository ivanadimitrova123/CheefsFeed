using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces;

public interface IUserGradesRepository
{
    Task<IEnumerable<UserGrades>> GetUserGradesByRecipeIdAsync(long recipeId);
    Task<UserGrades> GetUserGradeAsync(long userId, long recipeId);
    Task UpdateUserGradeAsync(UserGrades userGrade);
    Task AddUserGradeAsync(UserGrades userGrade);
}
