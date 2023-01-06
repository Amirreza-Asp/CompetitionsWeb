using Competitions.Domain.Entities.Managment;
using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Matches.Matches
{
    public class RegisterMatchDto
    {
        public int Number { get; set; }

        [Required(ErrorMessage = "شماره دانشجویی را وارد کنید")]
        [Range(100000000, 9999999999, ErrorMessage = "شماره دانشجویی صحیح نیست")]
        public long StudentNumber { get; set; }

        public IEnumerable<MatchDocument>? Documents { get; set; }
        public List<string> Files { get; set; } = new List<string>();
        public List<string> FilesNames { get; set; } = new List<string>();


        public List<(string Name, byte[] Bytes)> FilesBytes = new List<(string Name, byte[] Bytes)>();
    }
}
