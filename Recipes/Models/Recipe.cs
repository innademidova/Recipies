using System.Text.Json.Serialization;

namespace Recipes.Models;

public class Recipe
{
	public int Id { get; set; }
	public string Description { get; set; } = string.Empty;
	public User Author { get; set; } = null!;
	public int AuthorId { get; set; } 
	public DateTime CreatedAt { get; set; }
	public string ImageUrl { get; set; } = string.Empty;
}