namespace Competitions.Domain.Dtos.Matches.Matches
{
    public class MatchAwardListDto
    {
        public bool ReadOnly { get; set; }

        public Guid? Id { get; set; }

        public string Data { get; set; }

        public IEnumerable<MatchAwardDto>? Info { get; set; }
    }
}
