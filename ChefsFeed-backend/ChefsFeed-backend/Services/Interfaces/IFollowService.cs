using ChefsFeed_backend.Data.Models.Dtos;

namespace ChefsFeed_backend.Services.Interfaces;

public interface IFollowService
{
    bool IsFollowed(long userId, long followedUserId);
    string FollowUser(long userId, long followedUserId);
    string UnfollowUser(long userId, long followedUserId);
    List<UserDto> GetFollowingUsers(long userId, HttpContext httpContext);
    List<object> GetRecipesOfFollowedUsers(long userId, string requestScheme, string requestHost);
}
