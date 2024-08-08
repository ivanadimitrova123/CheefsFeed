using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Data;
using ChefsFeed_backend.Repositories.Interfaces;

namespace ChefsFeed_backend.Repositories.Implementation;

public class UserGradesRepository : IUserGradesRepository
{
    private readonly ApplicationDbContext _context;

    public UserGradesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserGrades> GetUserGradeAsync(long userId, long recipeId)
    {
        return await _context.UserGrades.SingleOrDefaultAsync(ug => ug.UserId == userId && ug.RecipeId == recipeId);
    }

    public async Task<IEnumerable<UserGrades>> GetUserGradesByRecipeIdAsync(long recipeId)
    {
        return await _context.UserGrades.Where(ug => ug.RecipeId == recipeId).ToListAsync();
    }

    public async Task AddUserGradeAsync(UserGrades userGrade)
    {
        await _context.UserGrades.AddAsync(userGrade);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserGradeAsync(UserGrades userGrade)
    {
        _context.UserGrades.Update(userGrade);
        await _context.SaveChangesAsync();
    }
}

