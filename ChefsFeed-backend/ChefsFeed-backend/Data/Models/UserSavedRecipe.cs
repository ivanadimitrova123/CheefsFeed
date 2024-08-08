namespace ChefsFeed_backend.Data.Models;

public class UserSavedRecipe
{

    public long UserId { get; set; }
    public long RecipeId { get; set; }
    [ForeignKey("UserId")]
    [JsonIgnore]
    public User User { get; set; }
    [ForeignKey("RecipeId")]
    [JsonIgnore]
    public Recipe Recipe { get; set; }


}