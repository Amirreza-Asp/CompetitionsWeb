namespace Competitions.Domain.Dtos.Matches.Matches
{
    public class MatchSecondInfoDto
    {
        public bool ReadOnly { get; set; }
        public Guid? Id { get; set; }

        public string? Image { get; set; }
        public string? Description { get; set; }


        public byte[]? ImageFile { get; set; }
    }
}
