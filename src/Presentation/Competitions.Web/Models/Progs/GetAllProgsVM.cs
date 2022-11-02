﻿using Competitions.Domain.Dtos.Extracurriculars;

namespace Competitions.Web.Models.Progs
{
    public class GetAllProgsVM
    {
        public IEnumerable<GetExtracurricularDetailsDto> Extracurriculars { get; set; }
        public Pagenation Pagenation { get; set; }
    }
}
