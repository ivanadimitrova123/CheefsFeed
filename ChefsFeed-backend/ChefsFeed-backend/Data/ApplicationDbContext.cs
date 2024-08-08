namespace ChefsFeed_backend.Data;

using ChefsFeed_backend.Data.Models;
using System.Collections.Generic;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserSavedRecipe> UserSavedRecipe { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.User)
            .WithMany(u => u.Recipes)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Followers)
            .WithMany(u => u.Following);

        modelBuilder.Entity<User>()
            .HasOne(u => u.ProfilePicture)
            .WithOne()
            .HasForeignKey<User>(u => u.ProfilePictureId);

        modelBuilder.Entity<Recipe>()
            .HasOne(u => u.Picture)
            .WithOne()
            .HasForeignKey<Recipe>(u => u.PictureId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>(comment =>
        {
            comment.HasKey(c => c.CommentId);
            comment.HasIndex(c => c.ParentId);

            comment.HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId);
        });

        modelBuilder.Entity<UserSavedRecipe>()
                .HasKey(usr => new { usr.UserId, usr.RecipeId });

        modelBuilder.Entity<UserSavedRecipe>()
            .HasOne(usr => usr.User)
            .WithMany(u => u.SavedRecepies)
            .HasForeignKey(usr => usr.UserId);

        modelBuilder.Entity<UserSavedRecipe>()
            .HasOne(usr => usr.Recipe)
            .WithMany(r => r.SavedRecepies)
            .HasForeignKey(usr => usr.RecipeId);

    }
}

