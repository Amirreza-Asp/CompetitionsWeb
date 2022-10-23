namespace Competitions.Domain.Dtos.Managment.Matches
{
    public class MatchConditionDto
    {
        public bool ReadOnly { get; set; }

        public Guid? Id { get; set; }

        public bool Free { get; set; } = true;
        public int? Payment { get; set; }
        public String? File { get; set; }

        public byte[]? RGFile { get; set; }
    }
}
