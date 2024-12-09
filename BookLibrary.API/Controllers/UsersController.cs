using Microsoft.AspNetCore.Mvc;
using MediatR;
using BookLibrary.Infrastructure.Services;
using BookLibrary.Application.Users.Commands.AddUser;
using BookLibrary.Application.Common;
using BookLibrary.Domain.Entities;
using BookLibrary.API.DTOs;
using BookLibrary.Domain.Interface;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPasswordService _passwordService;

    public UsersController(IMediator mediator, IPasswordService passwordService)
    {
        _mediator = mediator;
        _passwordService = passwordService;
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] AddUserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new User
        {
            Username = userDto.Username,
            Email = userDto.Email,
            PasswordHash = _passwordService.HashPassword(userDto.Password),
            CreatedAt = DateTime.UtcNow
        };

        var command = new AddUserCommand(user);
        var result = await _mediator.Send(command);

        if (result.Success)
        {
            return Ok(new { result.Message, UserId = result.Result.Id });
        }

        return BadRequest(result.Message);
    }
}
