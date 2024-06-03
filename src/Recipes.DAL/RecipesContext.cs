using Microsoft.EntityFrameworkCore;
using Recipes.DAL.Models;

namespace Recipes.DAL;

public sealed class RecipesContext : DbContext
{
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserSubscription> UserSubscriptions => Set<UserSubscription>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<RecipeFavorite> RecipeFavorites => Set<RecipeFavorite>();

    public RecipesContext(DbContextOptions<RecipesContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.Property(user => user.Email).HasMaxLength(1000);
            builder.HasIndex(user => user.Email).IsUnique();
            builder.HasOne(user => user.UserAccount)
                .WithOne()
                .HasForeignKey<UserAccount>(r => r.UserId);

            builder.Property(user => user.FirstName).HasMaxLength(300);
            builder.Property(user => user.LastName).HasMaxLength(300);
        });

        modelBuilder.Entity<UserAccount>(builder =>
        {
            builder.Property(userAccount => userAccount.Role)
                .HasConversion<string>()
                .HasMaxLength(100);

            builder.Property(userAccount => userAccount.Password).HasMaxLength(100);
        });
        
        modelBuilder.Entity<UserSubscription>(builder =>
        {
            builder.HasOne(e => e.Subscriber).WithMany()
                .HasForeignKey(e => e.SubscriberId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Subscription).WithMany()
                .HasForeignKey(e => e.SubscriptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasKey(e => new {e.SubscriberId, e.SubscriptionId});
        });
        
        modelBuilder.Entity<Comment>(builder =>
        {
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne<Recipe>()
                .WithMany()
                .HasForeignKey(r => r.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.Text)
                .HasMaxLength(1000);
        });
        
        modelBuilder.Entity<RecipeFavorite>(builder =>
        {
            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Recipe)
                .WithMany(p => p.Favorites)
                .HasForeignKey(p => p.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasKey(e => new {e.UserId, e.RecipeId}).IsClustered();
        });
    }
}