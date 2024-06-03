using Recipes.BLL.DTOs.Recipe;
using Recipes.ViewModels;

namespace Recipes.Mappers;

public static class RecipeMapper
{
    public static RecipeViewModel ToViewModel(this RecipeDto r)
    {
        return new RecipeViewModel(r.Id, r.Description, r.CreatedAt, r.AuthorFirstName,
            r.AuthorLastName, r.AuthorId, r.ImageUrl, r.FavoritesCount);
    }
}