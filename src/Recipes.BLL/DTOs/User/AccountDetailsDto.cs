using Recipes.DAL.Models;

namespace Recipes.BLL.DTOs.User;

public class AccountDetailsDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}