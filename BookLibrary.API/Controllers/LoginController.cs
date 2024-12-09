using Microsoft.AspNetCore.Mvc;
using BookLibrary.API.Helpers;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using BookLibrary.API.DTOs;

namespace BookLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenHelper _tokenHelper;
        private readonly IPasswordService _passwordService;

        public LoginController(IUserRepository userRepository, TokenHelper tokenHelper, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
            _passwordService = passwordService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userRepository.GetByUsernameAsync(login.Username);

            if (user == null || !_passwordService.VerifyPassword(login.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _tokenHelper.GenerateJwtToken(user.Username, "User");
            return Ok(new { token });
        }
    }
}
