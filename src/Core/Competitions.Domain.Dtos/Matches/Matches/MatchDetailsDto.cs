namespace Competitions.Domain.Dtos.Matches.Matches
{
    public class MatchDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        public string Sport { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
