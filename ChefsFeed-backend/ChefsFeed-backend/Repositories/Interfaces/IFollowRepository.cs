using ChefsFeed_backend.Data.Models;

namespace ChefsFeed_backend.Repositories.Interfaces;

public interface IFollowRepository
{
    User GetUserWithFollowersAndFollowing(long userId);
    User GetUserWithFollowing(long userId);
    User GetUserWithProfilePictureAndFollowing(long userId);
    User GetFollowedUserWithFollowing(long followedUserId);
    int GetCommentsCountForRecipe(long recipeId);
    void SaveChanges();
}

