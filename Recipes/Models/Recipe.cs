using System.Text.Json.Serialization;

namespace Recipes.Models;

public class Recipe
{
	public int Id { get; set; }
	public string Description { get; set; } = string.Empty;
	public string Author { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public string ImageUrl { get; set; } = string.Empty;
}