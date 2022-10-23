using Competitions.Domain.Dtos.Authentication.Positions;

namespace Competitions.Application.Authentication.Interfaces
{
    public interface IPositionAPI
    {
        Task<IEnumerable<UserByPosition>> GetUsersAsync ( String position );

        Task<IEnumerable<Position>> GetPositionsAsync ();
    }
}
