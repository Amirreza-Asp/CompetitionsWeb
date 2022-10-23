namespace Competitions.Domain.Dtos.Managment.Matches
{
    public class MatchSecondInfoDto
    {
        public bool ReadOnly { get; set; }
        public Guid? Id { get; set; }

        public String? Image { get; set; }
        public String? Description { get; set; }


        public Byte[]? ImageFile { get; set; }
    }
}
