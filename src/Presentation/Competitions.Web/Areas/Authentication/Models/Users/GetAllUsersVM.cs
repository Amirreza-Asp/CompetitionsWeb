using Competitions.Domain.Dtos.Authentication.User;

namespace Competitions.Web.Areas.Authentication.Models.Users
{
    public class GetAllUsersVM
    {
        public IEnumerable<UserDetails>? Users { get; set; }
        public UserFilter Filters { get; set; }
    }
}
