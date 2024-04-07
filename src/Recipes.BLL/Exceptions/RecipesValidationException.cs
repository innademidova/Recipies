namespace Recipes.BLL.Exceptions;

public class RecipesValidationException: Exception
{
    public RecipesValidationException(string message) : base(message)
    {
        
    }
}