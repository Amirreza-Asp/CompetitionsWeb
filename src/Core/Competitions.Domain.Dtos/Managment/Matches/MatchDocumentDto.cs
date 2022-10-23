using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Domain.Dtos.Managment.Matches
{
    public class MatchDocumentDto
    {
        public bool ReadOnly { get; set; }
        public Guid? Id { get; set; }

        public String Data { get; set; }

        public IEnumerable<DocumentDataDto>? Info { get; set; }

        public IEnumerable<SelectListItem>? Evidences { get; set; }

        public IEnumerable<SelectListItem> GetTypes () =>
            new List<SelectListItem>
            {
                new SelectListItem("pdf" , ".pdf"),
                new SelectListItem("word" , ".doc, .docx"),
                new SelectListItem("image" , "image/*"),
                new SelectListItem("zip" , ".zip"),
            };
    }
}
