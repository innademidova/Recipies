using FluentValidation;
using FluentValidation.AspNetCore;
using Recipes.Validators;
using Recipes.ViewModel;

namespace Recipes.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddFluentValidators(this IServiceCollection services)
	{
		services.AddFluentValidationAutoValidation();
		services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
		services.AddScoped<IValidator<CreateRecipeRequest>, CreateRecipeRequestValidator>();
		return services;
	}
}