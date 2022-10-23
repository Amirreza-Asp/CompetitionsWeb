using Competitions.Domain.Entities.Managment;
using System.ComponentModel.DataAnnotations;

namespace Competitions.Domain.Dtos.Managment.Matches
{
    public class RegisterMatchDto
    {
        public int Number { get; set; }

        [Required(ErrorMessage = "شماره دانشجویی را وارد کنید")]
        [Range(100000000 , 9999999999 , ErrorMessage = "شماره دانشجویی صحیح نیست")]
        public long StudentNumber { get; set; }

        public IEnumerable<MatchDocument>? Documents { get; set; }
        public List<String> Files { get; set; } = new List<String>();
        public List<String> FilesNames { get; set; } = new List<String>();
        public int TeamCount { get; set; }


        public List<(String Name, byte[] Bytes)>? FilesBytes;
    }
}
