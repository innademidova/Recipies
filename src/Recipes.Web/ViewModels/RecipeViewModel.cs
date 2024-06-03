namespace Recipes.ViewModels;


public record RecipeViewModel(int Id, string Description, DateTime CreatedAt, string AuthorFirstName, string AuthorLastName, int AuthorId, string ImageUrl, int FavoritesCount);
