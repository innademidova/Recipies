using System.ComponentModel.DataAnnotations;

namespace Recipes.ViewModel;

public record CreateRecipeRequest(string Description, string ImageUrl);