using Microsoft.AspNetCore.Mvc;
using BookLibrary.API.Helpers;
using BookLibrary.Domain.Interface;
using BookLibrary.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;

namespace BookLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenHelper _tokenHelper;
        private readonly PasswordService _passwordService;

        public LoginController(IUserRepository userRepository, TokenHelper tokenHelper, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
            _passwordService = (PasswordService?)passwordService;
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

    // DTO
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;
    }
}
