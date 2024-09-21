using ChefsFeed_backend.Data.Models;
using ChefsFeed_backend.Data;
using ChefsFeed_backend.Repositories.Interfaces;

namespace ChefsFeed_backend.Repositories.Implementation;

public class FollowRepository : IFollowRepository
{
    private readonly ApplicationDbContext _context;

    public FollowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public User GetUserWithFollowersAndFollowing(long userId)
    {
        return _context.Users
            .Include(u => u.Followers)
            .Include(u => u.Following)
            .FirstOrDefault(u => u.Id == userId);
    }

    public User GetUserWithFollowing(long userId)
    {
        return _context.Users
            .Include(u => u.Following)
            .ThenInclude(f => f.Recipes)
            .FirstOrDefault(u => u.Id == userId);
    }

    public User GetUserWithFollowingByCategory(long userId)
    {
        return _context.Users
            .Include(u => u.Following)
            .ThenInclude(f => f.Recipes)
           // .ThenInclude(r => r.Categories) 
            .FirstOrDefault(u => u.Id == userId);
    }

    public User GetUserWithProfilePictureAndFollowing(long userId)
    {
        return _context.Users
            .Include(u => u.Following)
            .Include(u => u.ProfilePicture)
            .FirstOrDefault(u => u.Id == userId);
    }

    public User GetFollowedUserWithFollowing(long followedUserId)
    {
        return _context.Users
            .Include(u => u.Following)
            .FirstOrDefault(u => u.Id == followedUserId);
    }
    public int GetCommentsCountForRecipe(long recipeId) 
    {
        return _context.Comments.Count(c => c.RecipeId == recipeId);
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}

