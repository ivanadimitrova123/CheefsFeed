using ChefsFeed_backend.Data.Models.Dtos;
using ChefsFeed_backend.Repositories.Interfaces;
using ChefsFeed_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChefsFeed_backend.Services.Implementation;

public class FollowService : IFollowService
{
    private readonly IFollowRepository _followRepository;

    public FollowService(IFollowRepository followRepository)
    {
        _followRepository = followRepository;
    }

    public bool IsFollowed(long userId, long followedUserId)
    {
        var user = _followRepository.GetUserWithFollowing(userId);
        return user?.Following.Any(u => u.Id == followedUserId) ?? false;
    }

    public string FollowUser(long userId, long followedUserId)
    {
        var user = _followRepository.GetUserWithFollowersAndFollowing(userId);
        if (user == null) return "User not found.";
        if (user.Id == followedUserId) return "You cannot follow yourself.";
        var followedUser = _followRepository.GetFollowedUserWithFollowing(followedUserId);
        if (followedUser == null) return "User to be followed not found.";
        if (user.Following.Contains(followedUser)) return "You are already following this user.";

        user.Following.Add(followedUser);
        followedUser.Followers.Add(user);
        _followRepository.SaveChanges();

        return "You are now following this user.";
    }

    public string UnfollowUser(long userId, long followedUserId)
    {
        var user = _followRepository.GetUserWithFollowersAndFollowing(userId);
        if (user == null) return "User not found.";
        var followedUser = _followRepository.GetFollowedUserWithFollowing(followedUserId);
        if (followedUser == null) return "User to be unfollowed not found.";
        if (!user.Following.Contains(followedUser)) return "You are not currently following this user.";

        user.Following.Remove(followedUser);
        followedUser.Followers.Remove(user);
        _followRepository.SaveChanges();

        return "You have unfollowed this user.";
    }

    public List<UserDto> GetFollowingUsers(long userId, HttpContext httpContext)
    {
        var user = _followRepository.GetUserWithProfilePictureAndFollowing(userId);
        if (user == null) return null;

        return user.Following.Select(u =>
        {
            var img = u.ProfilePictureId != null
                ? $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/image/{u.ProfilePictureId}"
                : null;
            return new UserDto(u.Id, u.Username, u.FirstName, u.LastName, u.Email, img, u.Role);
        }).ToList();
    }

    public List<object> GetRecipesOfFollowedUsers(long userId, string requestScheme, string requestHost)
    {
        var user = _followRepository.GetUserWithFollowing(userId);
        if (user == null) return null;

        return user.Following.SelectMany(followedUser => followedUser.Recipes.Select(recipe =>
        {
            var userImage = $"{requestScheme}://{requestHost}/api/image/{followedUser.ProfilePictureId}";
            return new
            {
                recipe = new
                {
                    recipe.Id,
                    recipe.Name,
                    recipe.PictureId,
                    RecipeImage = $"{requestScheme}://{requestHost}/api/image/{recipe.PictureId}",
                    Comments = _followRepository.GetCommentsCountForRecipe(recipe.Id),
                    recipe.Rating
                },
                user = new { userImage, followedUser.Username }
            };
        })).ToList<object>();
    }

    public List<dynamic> GetRecipesOfFollowedUsersByCategory(long userId, string requestScheme, string requestHost, long categoryId)
    {
        var user = _followRepository.GetUserWithFollowingByCategory(userId);
        if (user == null) return null;

        var recipesOfFollowedUsers = user.Following.SelectMany(followedUser => followedUser.Recipes
            .Where(recipe => recipe.CategoryId == categoryId)
            //.Where(recipe => recipe.Categories.Any(c => c.Id == categoryId))
            .Select(recipe => new
            {
                recipe = new
                {
                    recipe.Id,
                    recipe.Name,
                    recipe.PictureId,
                    RecipeImage = $"{requestScheme}://{requestHost}/api/image/{recipe.PictureId}",
                    Comments = _followRepository.GetCommentsCountForRecipe(recipe.Id),
                    recipe.Rating
                },
                user = new
                {
                    userImage = $"{requestScheme}://{requestHost}/api/image/{followedUser.ProfilePictureId}",
                    followedUser.Username
                }
            })).ToList<dynamic>();

        return recipesOfFollowedUsers;
    }

}