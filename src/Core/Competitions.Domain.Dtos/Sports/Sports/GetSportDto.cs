using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Domain.Dtos.Sports.Sports
{
    public class GetSportDto
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String Image { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
