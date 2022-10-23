namespace Competitions.Domain.Dtos.Authentication.User
{
    public class LoginResultDto
    {
        private LoginResultDto () { }

        public bool Success { get; set; }
        public string Message { get; set; }

        public static LoginResultDto Successful () => new LoginResultDto { Success = true };
        public static LoginResultDto Faild ( string message ) => new LoginResultDto { Success = false , Message = message };
    }
}
