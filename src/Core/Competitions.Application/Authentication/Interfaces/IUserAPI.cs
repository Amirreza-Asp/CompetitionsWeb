using Competitions.Domain.Dtos.Authentication.User;

namespace Competitions.Application.Authentication.Interfaces
{
    public interface IUserAPI
    {
        Task<UserInfo> GetUserAsync ( String nationalCode );

    }
}
