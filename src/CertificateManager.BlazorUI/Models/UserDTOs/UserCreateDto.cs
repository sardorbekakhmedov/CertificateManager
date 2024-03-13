using System.ComponentModel.DataAnnotations;
using CertificateManager.BlazorUI.Models.Enums;

namespace CertificateManager.BlazorUI.Models.UserDTOs;

public class UserCreateDto
{
    public string Username { get; set; } = null!;

    [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid age.")]
    public int Age { get; set; }

    public string? Email { get; set; }
    public EUserRoles? UserRole { get; set; }

    public string Password { get; set; } = null!;

    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string PasswordConfirm { get; set; } = null!;
}