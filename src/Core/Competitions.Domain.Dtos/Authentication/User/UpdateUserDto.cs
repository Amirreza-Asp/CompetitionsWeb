namespace Competitions.Domain.Dtos.Authentication.User
{
    public class UpdateUserDto : CreateUserDto
    {
        public Guid Id { get; set; }
    }
}
