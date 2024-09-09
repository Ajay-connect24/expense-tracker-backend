using ExpensesAPI.Entities.DataTransferObjects;

namespace ExpensesAPI.Contracts
{
    

    public interface IUserRepository
    {
        Task<UserInfoDto> GetUserInfoAsync();
    }
}
