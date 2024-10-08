﻿namespace ChefsFeed_backend.Data.Models;

public class ReportedRecipe
{
    public long UserId { get; set; }
    public long RecipeId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }
}