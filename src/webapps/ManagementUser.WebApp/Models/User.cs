using Microsoft.AspNetCore.Identity;

namespace ManagementUser.WebApp.Models;

public class User : IdentityUser
{
    public Guid Id { get; set; }
    public new string? UserName { get; set; }
    public new string? Email { get; set; }
    public string? Password { get; set; }
    public string? ReturnUrl { get; set; }
    public string? ConfirmPassword { get; set; }
}