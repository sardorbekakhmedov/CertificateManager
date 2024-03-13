using System.Security.Claims;
using CertificateManager.Application.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CertificateManager.Application.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUser(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    private HttpContext? Context => _contextAccessor.HttpContext;

    public string? Username
    {
        get
        {
            var userName = Context?.User.FindFirst(ClaimTypes.Name)?.Value;

            return userName ?? null;
            // throw new UnauthorizedException("Username is not available. Authentication Failed."); ;
        }
    }

    public Guid? UserId
    {
        get
        {
            var userIdValue = Context?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdValue, out Guid userId))
            {
                return userId;
            }
            else
            {
                return null;
            }

            // throw new UnauthorizedException("User ID is not available. Authentication Failed."); ;
        }
    }
}