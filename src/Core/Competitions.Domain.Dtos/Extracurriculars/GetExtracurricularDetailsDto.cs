namespace Competitions.Domain.Dtos.Extracurriculars
{
    public class GetExtracurricularDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Sport { get; set; }
        public string AudienceType { get; set; }
        public string Place { get; set; }
    }
}
