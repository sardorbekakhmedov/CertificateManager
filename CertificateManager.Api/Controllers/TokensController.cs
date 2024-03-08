using Certificate.Application.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Certificate.Application.Services.TokenServices;

namespace CertificateManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokensController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokensController(ITokenService tokenService, ICurrentUser currentUser)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(TokenRequest dto)
    {
        if (ModelState.IsValid == false)
            return BadRequest(ModelState);

        return Ok( await _tokenService.GetTokenAsync(dto));
    }

}