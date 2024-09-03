using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces;

public interface IFollowRepository
{
    User GetUserWithProfilePictureAndFollowing(long userId);
    User GetFollowedUserWithFollowing(long followedUserId);
    User GetUserWithFollowersAndFollowing(long userId);
    User GetUserWithFollowingByCategory(long userId);
    int GetCommentsCountForRecipe(long recipeId);
    User GetUserWithFollowing(long userId);
    void SaveChanges();
}

