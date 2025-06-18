using Microsoft.AspNetCore.Mvc;
using ecotrip_backend.Auth.Application.DTOs; 
namespace ecotrip_backend.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <param name="request"
        [HttpPost("register")] 
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.UserType))
            {
                return BadRequest("Email, Password y UserType son requeridos.");
            }

            if (request.UserType != "Tourist" && request.UserType != "Agency")
            {
                return BadRequest("UserType debe ser 'Tourist' o 'Agency'.");
            }
            var token = $"simulated-jwt-token-for-{request.Email}-{request.UserType}";

            var response = new AuthResponse
            {
                Token = token,
                UserType = request.UserType,
                Email = request.Email
            };

            return Ok(response);
        }

        /// <param name="request"
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return Unauthorized("Credenciales inválidas."); // En un caso real, esto sería 401
            }

            string userType = "Tourist"; 
            if (request.Email.Contains("agency"))
            {
                userType = "Agency";
            }

            var token = $"simulated-jwt-token-for-{request.Email}-{userType}";

            var response = new AuthResponse
            {
                Token = token,
                UserType = userType,
                Email = request.Email
            };

            return Ok(response);
        }
    }
}