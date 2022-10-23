using Competitions.Domain.Dtos.Managment.Extracurriculars;

namespace Competitions.Web.Areas.Managment.Models.Extracurriculars
{
    public class GetAllExtracurricularsVM
    {
        public IEnumerable<GetExtracurricularDetailsDto> Extracurriculars { get; set; }
        public ExtracurricularFilter Filter { get; set; }
    }
}
