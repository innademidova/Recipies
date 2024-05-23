namespace Recipes.DAL.Models;

public class Comment
{
    public int Id { get; init; }
    public string Text { get; init; } = null!;
    public DateTime UpdatedAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public int AuthorId { get; init; }
    public int RecipeId { get; init; }
}