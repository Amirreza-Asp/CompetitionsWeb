using Competitions.Domain.Entities.Managment;

namespace Competitions.Web.Models.Home
{
    public class MatchHomeVM
    {
        public IEnumerable<Match> Matches { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }
    }
}
