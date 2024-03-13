using System.ComponentModel.DataAnnotations;

namespace CertificateManager.BlazorUI.Models.Enums;

public enum EUserRoles
{
    [Display(Name = "Admin")]
    Admin = 1,

    [Display(Name = "SuperUser")]
    SuperUser,

    [Display(Name = "User")]
    User
}