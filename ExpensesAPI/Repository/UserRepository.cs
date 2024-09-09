using ExpensesAPI.Contracts;
using ExpensesAPI.Entities.DataTransferObjects;
using Mapster;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExpensesAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserInfoDto> GetUserInfoAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            // Check if the user is authenticated
            if (httpContext?.User == null || !httpContext.User.Identity.IsAuthenticated)
                return null;

            // Use Mapster to map ClaimsPrincipal to UserInfoDto
            var userInfoDto = httpContext.User.Adapt<UserInfoDto>();

            return userInfoDto;
        }
    }
}