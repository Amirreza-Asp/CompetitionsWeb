using Microsoft.AspNetCore.Mvc.Rendering;

namespace Competitions.Domain.Dtos.Matches.Matches
{
    public class MatchDocumentDto
    {
        public bool ReadOnly { get; set; }
        public Guid? Id { get; set; }

        public string Data { get; set; }

        public IEnumerable<DocumentDataDto>? Info { get; set; }

        public IEnumerable<SelectListItem>? Evidences { get; set; }

        public IEnumerable<SelectListItem> GetTypes() =>
            new List<SelectListItem>
            {
                new SelectListItem("pdf" , ".pdf"),
                new SelectListItem("word" , ".doc, .docx"),
                new SelectListItem("image" , "image/*"),
                new SelectListItem("zip" , ".zip"),
            };
    }
}
