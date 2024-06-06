using Recipes.BLL.DTOs.User;
using Recipes.DAL.Models;

namespace Recipes.BLL.Mappers;

public static class UserMapper
{
    public static AccountDetailsDto ToDto(this User u)
    {
        return new AccountDetailsDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Role = u.UserAccount.Role,
            CreatedAt = u.CreatedAt
        };
    }
}