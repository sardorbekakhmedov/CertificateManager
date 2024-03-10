﻿using System.ComponentModel.DataAnnotations;
using CertificateManager.UI.Models.Enums;

namespace CertificateManager.UI.Models.UserDTOs;

public class UserUpdateDto
{
    public string? Username { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid age.")]
    public int Age { get; set; }
    public string? Email { get; set; }
    public EUserRoles? UserRole { get; set; }
}