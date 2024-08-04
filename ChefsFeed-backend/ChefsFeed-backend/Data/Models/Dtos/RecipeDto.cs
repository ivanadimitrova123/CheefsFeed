namespace ChefsFeed_backend.Data.Models.Dtos;

public class RecipeDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string RecipeImage { get; set; }
    public int Comments { get; set; }

}
