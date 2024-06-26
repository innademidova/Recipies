﻿using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Recipes.BLL.Authentication;
using Recipes.Validators;
using Recipes.ViewModel;
using Recipes.ViewModels;
using Swashbuckle.AspNetCore.Filters;

namespace Recipes.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddFluentValidators(this IServiceCollection services)
	{
		services.AddFluentValidationAutoValidation();
		services.AddScoped<IValidator<RegistrationRequest>, CreateUserRequestValidator>();
		services.AddScoped<IValidator<CreateRecipeRequest>, CreateRecipeRequestValidator>();
		return services;
	}
	
	public static IServiceCollection AddSwaggerWithAuthorization(this IServiceCollection services)
	{
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "My API",
				Version = "v1"
			});
			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Please enter a valid JWT token (without prefix 'Bearer')",
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				BearerFormat = "JWT",
				Scheme = "Bearer"
			});
			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string[] { }
				}
			});
			c.ExampleFilters();
		});
		
		services.AddSwaggerExamplesFromAssemblyOf<Program>();
		
		return services;
	}
	
	public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services)
	{
		services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = AuthOptions.Issuer,
				ValidAudience = AuthOptions.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(AuthOptions.Key))
			});
		return services;
	}
}