using Competitions.Domain.Entities.Managment;

namespace Competitions.Web.Models.Calenders
{
    public class GetAllProgCalenders
    {
        public ProgCalenderFilter Filters { get; set; }
        public IEnumerable<Extracurricular> Progs { get; set; }
    }
}
