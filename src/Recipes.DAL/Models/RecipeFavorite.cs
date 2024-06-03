namespace Recipes.DAL.Models;

public class RecipeFavorite
{
    public Recipe Recipe { get; set; } = null!;
    public int RecipeId { get; set; }
    public User User { get; set; } = null!;
    public int UserId { get; set; }
}