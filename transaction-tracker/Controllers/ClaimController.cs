using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using transaction_tracker.Entities.Models;
using Microsoft.AspNetCore.Authorization;

namespace transaction_tracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        [HttpGet("userDetails")]
        public ActionResult<UserDetailsDto> GetUserDetailsFromClaims()
        {
            // Retrieve the claims from the JWT token
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

            if (claimsIdentity == null)
            {
                return Unauthorized();
            }

            // Extract claims
            var claims = claimsIdentity.Claims;

            // Create a UserDetailsDto from claims
            var userDetailsDto = new UserDetailsDto
            {
                Id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                UserName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
            };

            // Return the UserDetailsDto
            return Ok(userDetailsDto);
        }
    }
}