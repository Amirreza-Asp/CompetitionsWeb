using Competitions.Domain.Dtos.Extracurriculars;

namespace Competitions.Web.Areas.Managment.Models.Extracurriculars
{
    public class GetAllExtracurricularsVM
    {
        public IEnumerable<GetExtracurricularDetailsDto> Extracurriculars { get; set; }
        public ExtracurricularFilter Filter { get; set; }
    }
}
