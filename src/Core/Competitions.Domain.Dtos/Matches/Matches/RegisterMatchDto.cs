using Competitions.Domain.Entities.Managment;
using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Matches.Matches
{
    public class RegisterMatchDto
    {
        public int Number { get; set; }

        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MinLength(10, ErrorMessage = "کد ملی 10 رقمی است")]
        [MaxLength(10, ErrorMessage = "کد ملی 10 رقمی است")]
        public String NationalCode { get; set; }

        public IEnumerable<MatchDocument>? Documents { get; set; }
        public List<string> Files { get; set; } = new List<string>();
        public List<string> FilesNames { get; set; } = new List<string>();


        public List<(string Name, byte[] Bytes)> FilesBytes = new List<(string Name, byte[] Bytes)>();
    }
}
