using Competitions.Domain.Dtos.Authentication.User;

namespace Competitions.Application.Authentication.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResultDto> RegisterAsync(RegisterDto command);

        Task<LoginResultDto> LoginAsync(LoginDto command);

        Task ChangePasswordAsync(ChangePasswordDto command);

        Task KhemdatLoginAsync(String nationalCode);
    }
}
