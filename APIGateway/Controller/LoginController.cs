using APIGateway.Model;
using APIGateway.TokenHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly List<(string Username, string Password)> _users = new List<(string, string)>
        {
            ("user1", "password1"),
            ("user2", "password2")
        };
        private readonly ITokenHandler _tokenHandler;

        public LoginController(ITokenHandler tokenHandler)
        {
            _tokenHandler = tokenHandler;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestModel loginRequest)
        {
            var user = _users.FirstOrDefault(u => u.Username == loginRequest.Username && u.Password == loginRequest.Password);
            if (user == default)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }
            var token = _tokenHandler.CreateToken(user.Username);
            return Ok(new LoginResponseModel { Token = token });
        }

    }
}
