using AccountManagementSystem.Models;
using AccountManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserModel model)
        {
            var result = await _authService.RegisterUser(model);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _authService.LoginAsync(model);
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetUsers();

            if (users.IsSuccess)
            {
                return Ok(users);
            }

            return BadRequest(users);
        }

    }
}
