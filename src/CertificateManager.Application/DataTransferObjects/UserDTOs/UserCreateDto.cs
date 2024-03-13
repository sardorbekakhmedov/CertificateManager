using System.ComponentModel.DataAnnotations;
using CertificateManager.Domain.Enums;

namespace CertificateManager.Application.DataTransferObjects.UserDTOs;

public class UserCreateDto
{
    public required string Username { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid age.")]
    public int Age { get; set; }
    public string? Email { get; set; }
    public EUserRoles? UserRole { get; set; }
    public required string Password { get; set; }
    [Compare("Password")]
    public required string PasswordConfirm { get; set; }
}