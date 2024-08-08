using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces;

public interface IUserGradesRepository
{
    Task<UserGrades> GetUserGradeAsync(long userId, long recipeId);
    Task<IEnumerable<UserGrades>> GetUserGradesByRecipeIdAsync(long recipeId);
    Task AddUserGradeAsync(UserGrades userGrade);
    Task UpdateUserGradeAsync(UserGrades userGrade);
}
