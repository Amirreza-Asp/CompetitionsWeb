﻿using Competitions.Domain.Entities.Sports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.Persistence.Sports.Mappings
{
    public class CoachEvidenceTypeMappings : IEntityTypeConfiguration<CoachEvidenceType>
    {
        public void Configure(EntityTypeBuilder<CoachEvidenceType> builder)
        {

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Title);
            builder.Property(u => u.Description).IsRequired(false);
        }
    }
}
