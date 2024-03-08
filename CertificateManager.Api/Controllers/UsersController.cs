using Certificate.Application.Abstractions.Interfaces;
using Certificate.Application.Abstractions.Interfaces.RepositoryServices;
using Certificate.Application.DataTransferObjects.UserDTOs;
using Certificate.Application.SortFilters.FilterEntities;
using CertificateManager.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CertificateManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICurrentUser _currentUser;

    public UsersController(IUserService userService, ICurrentUser currentUser)
    {
        _userService = userService;
        _currentUser = currentUser;
    }

    [HttpPost("create")]
    [Authorize(Policy = nameof(EUserRoles.SuperUser))]
    public async Task<IActionResult> Create(UserCreateDto dto)
    {
        if (ModelState.IsValid == false)
            return BadRequest(ModelState);

        return Created("Register", new { Id = await _userService.CreateAsync(dto) });
    }


    [HttpGet("list")]
    [Authorize(Policy = nameof(EUserRoles.User))]
    public async Task<IActionResult> GetAll([FromQuery] UserFilter filter)
    {
        if (ModelState.IsValid == false)
            return BadRequest(ModelState);

        return Ok(await _userService.GetAllAsync(filter));
    }

    [HttpGet("{userId:guid}")]
    [Authorize(Policy = nameof(EUserRoles.User))]
    public async Task<IActionResult> GetById(Guid userId)
    {
        if (ModelState.IsValid == false)
            return BadRequest(ModelState);

        return Ok(await _userService.GetByIdAsync(userId));
    }

    [HttpGet("{username}")]
    [Authorize(Policy = nameof(EUserRoles.User))]
    public async Task<IActionResult> GetByUsername(string username)
    {
        if (ModelState.IsValid == false)
            return BadRequest(ModelState);

        return Ok(await _userService.GetByUsernameAsync(username));
    }

    [HttpGet("profile")]
    [Authorize(Policy = nameof(EUserRoles.User))]
    public async Task<IActionResult> GetProfileData()
    {
        if (ModelState.IsValid == false)
            return BadRequest(ModelState);

        return Ok(await _userService.GetByIdAsync(_currentUser.UserId));
    }


    [HttpPut("{userId:guid}")]
    [Authorize(Policy = nameof(EUserRoles.Admin))]
    public async Task<IActionResult> Update(Guid userId, UserUpdateDto dto)
    {
        if (ModelState.IsValid == false)
            return BadRequest(ModelState);

        await _userService.UpdateAsync(userId, dto);
        return Ok();
    }

    [HttpDelete("{userId:guid}")]
    [Authorize(Policy = nameof(EUserRoles.Admin))]
    public async Task<IActionResult> Delete(Guid userId)
    {
        await _userService.DeleteAsync(userId);
        return Ok();
    }
}