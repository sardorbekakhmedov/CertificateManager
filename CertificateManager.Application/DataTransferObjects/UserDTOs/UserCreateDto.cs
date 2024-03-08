using System.ComponentModel.DataAnnotations;

namespace Certificate.Application.DataTransferObjects.UserDTOs;

public class UserCreateDto
{
    public required string Username { get; set; }
    public uint Age { get; set; }
    public string? Email { get; set; }
    public required string Password { get; set; }
    [Compare("Password")]
    public required string PasswordConfirm { get; set; }
}