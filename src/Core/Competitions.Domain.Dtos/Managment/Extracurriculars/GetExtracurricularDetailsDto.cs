namespace Competitions.Domain.Dtos.Managment.Extracurriculars
{
    public class GetExtracurricularDetailsDto
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Sport { get; set; }
        public String AudienceType { get; set; }
        public String Place { get; set; }
    }
}
