namespace ChefsFeed_backend.Data.Models;
public class Category
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    public string Name { get; set; }
}