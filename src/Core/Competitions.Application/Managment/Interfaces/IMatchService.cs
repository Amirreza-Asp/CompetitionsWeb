using Competitions.Domain.Dtos.Matches.Matches;

namespace Competitions.Application.Managment.Interfaces
{
    public interface IMatchService
    {
        Task CreateAsync ( MatchFirstInfoDto firstInfo , MatchSecondInfoDto secondInfo , MatchConditionDto condition
            , MatchDocumentDto document , MatchAwardListDto award );

        Task UpdateAsync ( MatchFirstInfoDto firstInfo , MatchSecondInfoDto secondInfo , MatchConditionDto condition
            , MatchDocumentDto document , MatchAwardListDto award );

        Task RemoveAsync ( Guid id );
    }
}
