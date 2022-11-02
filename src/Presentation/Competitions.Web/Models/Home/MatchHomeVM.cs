using Competitions.Domain.Entities.Managment;
using Competitions.Domain.Entities.Notifications;

namespace Competitions.Web.Models.Home
{
    public class MatchHomeVM
    {
        public IEnumerable<Match> Matches { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }
    }
}
