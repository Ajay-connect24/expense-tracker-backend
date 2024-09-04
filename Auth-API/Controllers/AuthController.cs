using Auth_API.Contracts;
using Auth_API.Entities.DataTransferObjects;
using Auth_API.Entities.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var registerModel = registerDto.Adapt<Register>();
            var response = await _authRepository.RegisterAsync(registerModel);

            if (response.Status == "Error") {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
            
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
          
            var loginModel = loginDto.Adapt<Login>();

            var token = await _authRepository.LoginAsync(loginModel);

            if (token == null)
            {
                return Unauthorized(new { Status = "Error", Message = "Invalid login attempt." });
            }

            return Ok( token );
        }
    }
}


//[Authorize(Roles = UserRoles.User)]
//[HttpGet("test")]
//public async Task<IActionResult> Test()
//{
//    return Ok(_userManager.Users.ToList());
//}