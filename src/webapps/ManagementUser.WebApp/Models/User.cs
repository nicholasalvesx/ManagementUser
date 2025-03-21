using Microsoft.AspNetCore.Identity;

namespace ManagementUser.WebApp.Models;

public class User : IdentityUser
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ReturnUrl { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? Role { get; set; }
}