namespace Recipes.BLL.DTOs.Recipe;

public record RecipeDto(int Id, string Description, DateTime CreatedAt, string AuthorFirstName, string AuthorLastName, 
    int AuthorId, string ImageUrl, int FavoritesCount, bool IsAddedToFavoriteByCurrentUser);