namespace Competitions.Domain.Dtos.Managment.Matches
{
    public class MatchDetailsDto
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Status { get; set; }
        public String Gender { get; set; }
        public String Sport { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
