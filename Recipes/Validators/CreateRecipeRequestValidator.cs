using FluentValidation;
using Recipes.ViewModel;

namespace Recipes.Validators;

public class CreateRecipeRequestValidator : AbstractValidator<CreateRecipeRequest>
{
    public CreateRecipeRequestValidator()
    {
        RuleFor(e => e.Description).NotEmpty();
    }
}