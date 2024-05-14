using FluentValidation;
using Recipes.ViewModels;

namespace Recipes.Validators;

public class CreateRecipeRequestValidator : AbstractValidator<CreateRecipeRequest>
{
    public CreateRecipeRequestValidator()
    {
        RuleFor(e => e.Description).NotEmpty();
    }
}