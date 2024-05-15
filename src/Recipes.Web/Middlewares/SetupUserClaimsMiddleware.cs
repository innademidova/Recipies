using System.Security.Claims;
using Recipes.BLL.Interfaces;
using Recipes.DAL.Models;

namespace Recipes.Middlewares;

public class SetupUserClaimsMiddleware
{
    private readonly RequestDelegate _next;

    public SetupUserClaimsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentUser currentUser)
    {
        var user = context.User;

        if (user.Identity?.IsAuthenticated == false)
        {
            await _next(context);
        }
        else
        {
            var userIdFromToken = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var id = int.Parse(userIdFromToken ?? throw new InvalidOperationException("Can not retrieve user id from token"));
            var email = user.FindFirstValue(ClaimTypes.Email);
            var role = user.FindFirstValue(ClaimTypes.Role);
            var isBanned = user.Claims.FirstOrDefault(c => c.Type == "IsBanned")?.Value;

            Enum.TryParse(role, out UserRole userRole);
            bool.TryParse(isBanned, out bool bannedStatus);
            currentUser.Id = id;
            currentUser.Email = email!;
            currentUser.Role = userRole;
            currentUser.IsBanned = bannedStatus;

            await _next(context);
        }
    }
}