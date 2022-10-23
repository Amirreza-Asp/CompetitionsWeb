using Competitions.Domain.Entities.Sports;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Domain.Dtos.Sports.Sports
{
    public class UpdateSportDto : CreateSportDto
    {
        public long Id { get; set; }
    }
}
