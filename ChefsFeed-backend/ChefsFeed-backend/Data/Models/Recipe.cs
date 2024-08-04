namespace ChefsFeed_backend.Data.Models;

using Newtonsoft.Json;

public class Recipe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [Required(ErrorMessage = "Field can't be empty")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Field can't be empty")]
    public string Description { get; set; }
    [ForeignKey("PictureId")]
    public long? PictureId { get; set; }

    [JsonIgnore]
    public Picture? Picture { get; set; }
    [Required(ErrorMessage = "Field can't be empty")]
    public List<string> Ingredients { get; set; }

    [ForeignKey("UserId")]
    public long UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }

    public string Level { get; set; } = string.Empty;
    public string Prep { get; set; } = string.Empty;
    public string Cook { get; set; } = string.Empty;
    public string Total { get; set; } = string.Empty;
    public string Yield { get; set; } = string.Empty;
    public float Rating { get; set; } = 0;

    public ICollection<Category> Categories { get; set; } = new List<Category>();


}