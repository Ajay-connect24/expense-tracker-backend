using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExpensesAPI.Contracts;
using ExpensesAPI.Entities.DataTransferObjects;

namespace ExpensesAPI.Controllers
{
    [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userInfo = await _userRepository.GetUserInfoAsync();
            if (userInfo == null)
                return NotFound("User not found.");

            return Ok(userInfo);
        }
    }
}