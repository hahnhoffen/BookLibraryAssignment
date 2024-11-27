using Microsoft.AspNetCore.Mvc;
using BookLibrary.API.Helpers;

namespace BookLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly TokenHelper _tokenHelper;

        public LoginController(TokenHelper tokenHelper)
        {
            _tokenHelper = tokenHelper;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel login)
        {
            // Hardcoded user validation for demonstration (replace with your own logic)
            if (login.Username == "admin" && login.Password == "password")
            {
                var token = _tokenHelper.GenerateJwtToken(login.Username, "Admin");
                return Ok(new { token });
            }

            return Unauthorized("Invalid credentials");
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
