using ChefsFeed_backend.Data.Models.Dtos;

namespace ChefsFeed_backend.Services.Interfaces;

public interface IFollowService
{
    List<dynamic> GetRecipesOfFollowedUsersByCategory(long userId, string requestScheme, string requestHost, long categoryId);
    List<object> GetRecipesOfFollowedUsers(long userId, string requestScheme, string requestHost);
    List<UserDto> GetFollowingUsers(long userId, HttpContext httpContext);
    string UnfollowUser(long userId, long followedUserId);
    string FollowUser(long userId, long followedUserId);
    bool IsFollowed(long userId, long followedUserId);


}
