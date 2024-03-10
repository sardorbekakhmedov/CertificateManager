using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace CertificateManager.UI.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ILocalStorageService _localStorage;

        public CustomAuthenticationStateProvider(ILocalStorageService ILocalStorageService)
        {
            _localStorage = ILocalStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("token");
            ClaimsIdentity identity;
            if (token is not null)
            {
                var claims = ParseToken(token).ToList();

                var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
                var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var userName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                await _localStorage.SetItemAsync("username",userName);
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Role, roleClaim),
                    new Claim(ClaimTypes.Name, userName),
                }, "apiauth_type");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var user = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(user));
        }

        public void MarkUserAsAuthenticated(string email, string role)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            }, "apiauth_type");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private IEnumerable<Claim> ParseToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            return jsonToken?.Claims ?? new List<Claim>();
        }
    }
}
