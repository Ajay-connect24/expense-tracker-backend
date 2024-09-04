using Auth_API.Entities.DataTransferObjects;
using Auth_API.Entities.Models;

namespace Auth_API.Contracts
{
    public interface IAuthRepository
    {
        Task<Response> RegisterAsync(Register registerModel);
        Task<string> LoginAsync(Login loginModel);
    }
}
