using FluentValidation;
using Recipes.ViewModel;

namespace Recipes.Validators;

public class CreateUserRequestValidator: AbstractValidator<RegistrationRequest>
{
		public CreateUserRequestValidator()
		{
			RuleFor(e => e.Email).NotEmpty().EmailAddress();
			RuleFor(e => e.FirstName).NotEmpty().MaximumLength(300);
			RuleFor(e => e.LastName).NotEmpty().MaximumLength(300);
		}
}