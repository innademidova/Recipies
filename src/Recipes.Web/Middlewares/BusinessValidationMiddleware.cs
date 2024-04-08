using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Recipes.BLL.Exceptions;

namespace Recipes.Middlewares;

public class BusinessValidationMiddleware
{
    private readonly RequestDelegate _next;

    public BusinessValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (RecipesValidationException ex)
        {
            var error = new ValidationProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Title = ex.Message
            };
            
            context.Response.ContentType = MediaTypeNames.Text.Plain;
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}
