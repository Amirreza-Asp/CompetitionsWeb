using Competitions.Domain.Entities.Authentication;
using Competitions.Domain.Entities.Managment;

namespace Competitions.Web.Areas.Matches.Models.Matches
{
    public class UserInfoVM
    {
        public User User { get; set; }
        public UserTeam UserTeam { get; set; }
    }
}
