using Competitions.Domain.Dtos.Authentication;
using Competitions.Domain.Dtos.Authentication.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Competitions.Application.Authentication.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResultDto> RegisterAsync(RegisterDto command);

        Task<LoginResultDto> LoginAsync(ProfileRequest command, bool NeedCompleteInfo = false);

        Task<List<Claim>> LoginWithSSOAsync(JwtSecurityToken ssoTokens);

        Task ChangePasswordAsync(ChangePasswordDto command);

        Task KhemdatLoginAsync(String nationalCode);
    }
}
