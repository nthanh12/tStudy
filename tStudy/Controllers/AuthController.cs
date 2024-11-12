using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tStudy.Application.Interfaces;
using tStudy.Models.DTOs;

namespace tStudy.Controllers
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
        public async Task<IActionResult> Register(SystemRegisterUserDTO user)
        {
            var result = await _authService.RegisterSystemUser(user);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(SystemSignInUserDTO credentials)
        {
            var result = await _authService.LoginSystemUser(credentials);

            if (result.Success)
            {
                return Ok(result);
            }
            return Unauthorized(result);
        }
    }
}
