using System.Security.Claims;
using Certificate.Application.Abstractions.Interfaces;
using Certificate.Application.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Certificate.Application.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUser(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    private HttpContext? Context => _contextAccessor.HttpContext;

    public string Username
    {
        get
        {
            var userName = Context?.User.FindFirst(ClaimTypes.Name)?.Value;

            if (userName is not null)
            {
                return userName;
            }

            throw new UnauthorizedException("Username is not available. Authentication Failed."); ;
        }
    }

    public Guid UserId
    {
        get
        {
            var userIdValue = Context?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdValue, out Guid userId))
            {
                return userId;
            }

            throw new UnauthorizedException("User ID is not available. Authentication Failed."); ;
        }
    }
}