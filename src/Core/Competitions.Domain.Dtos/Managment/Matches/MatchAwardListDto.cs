namespace Competitions.Domain.Dtos.Managment.Matches
{
    public class MatchAwardListDto
    {
        public bool ReadOnly { get; set; }

        public Guid? Id { get; set; }

        public String Data { get; set; }

        public IEnumerable<MatchAwardDto>? Info { get; set; }
    }
}
