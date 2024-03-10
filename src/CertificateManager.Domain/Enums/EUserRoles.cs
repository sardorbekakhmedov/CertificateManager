using System.ComponentModel.DataAnnotations;

namespace CertificateManager.Domain.Enums;

public enum EUserRoles
{
    [Display(Name = "Admin")]
    Admin = 1,

    [Display(Name = "SuperUser")]
    SuperUser,

    [Display(Name = "User")]
    User
}