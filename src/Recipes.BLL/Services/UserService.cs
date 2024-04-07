using Microsoft.EntityFrameworkCore;
using Recipes.BLL.Authentication;
using Recipes.BLL.Exceptions;
using Recipes.DAL;

namespace Recipes.BLL.Services;

public class UserService
{
    private readonly JwtTokenGenerator _tokenGenerator = new();
    private readonly RecipesContext _context;

    public UserService(RecipesContext context)
    {
        _context = context;
    }

    public async Task<(int UserId, string AccessToken)> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            throw new RecipesValidationException("Email or password are incorrect");
        }

        if (user.Password != password)
        {
            throw new RecipesValidationException("Email or password are incorrect");
        }

        var accessToken = _tokenGenerator.GenerateToken(user);

        return (user.Id, accessToken);
    }
}