namespace Competitions.Domain.Dtos.Authentication.User
{
    public class UserDetails
    {
        public Guid Id { get; set; }
        public String FullName { get; set; }
        public String NationalCode { get; set; }
        public String PhoneNumber { get; set; }
        public String Role { get; set; }
    }
}
