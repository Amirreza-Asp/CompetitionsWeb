namespace Competitions.Domain.Dtos.Authentication.User
{
    public class RegisterResultDto
    {
        private RegisterResultDto () { }

        public bool Success { get; set; }
        public string Message { get; set; }

        public static RegisterResultDto Successful () => new RegisterResultDto { Success = true };
        public static RegisterResultDto Faild ( string message ) => new RegisterResultDto { Success = false , Message = message };
    }
}
