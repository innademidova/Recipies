using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Recipes.BLL.Authentication;
using Recipes.BLL.Exceptions;
using Recipes.DAL;
using Recipes.DAL.Models;

namespace Recipes.BLL.Services;

public class UserService
{
    private readonly JwtTokenGenerator _tokenGenerator = new();
    private readonly RecipesContext _context;

    public UserService(RecipesContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetRecipes()
    {
        return await _context.Users.Include(u => u.UserAccount).ToListAsync();
    }
    public async Task<Result<(int UserId, string AccessToken)>> Register(string firstName, string lastName, string email, string password)
    {
        var existedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (existedUser != null)
        {
            return new Result<(int UserId, string AccessToken)>(new RecipesValidationException("Unhandled exception: user already exists"));
        }
        
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User
        {
            Email = email,
            CreatedAt = DateTime.UtcNow,
            FirstName = firstName,
            LastName = lastName,
            UserAccount = new UserAccount
            {
                Password = passwordHash,
            },
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var accessToken = _tokenGenerator.GenerateToken(user);

        return (user.Id, accessToken);
    }
    public async Task<Result<(int UserId, string AccessToken)>> Login(string email, string password)
    {
        var user = await _context.Users.Include(u => u.UserAccount).FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return new Result<(int UserId, string AccessToken)>(new RecipesValidationException("Email or password are incorrect"));
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.UserAccount.Password))
        {
            return new Result<(int UserId, string AccessToken)>(new RecipesValidationException("Email or password are incorrect"));
        }

        var accessToken = _tokenGenerator.GenerateToken(user);

        return (user.Id, accessToken);
    }
}