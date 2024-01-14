using Microsoft.EntityFrameworkCore;
using Recipes.Models;

namespace Recipes;

public sealed class RecipesContext : DbContext
{
	public DbSet<Recipe> Recipes => Set<Recipe>();
	public DbSet<User> Users => Set<User>();
	
	public RecipesContext(DbContextOptions<RecipesContext> options) : base(options)
	{
	}
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>()
			.HasIndex(u => u.Email)
			.IsUnique();
	} 
}