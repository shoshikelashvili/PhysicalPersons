using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    class PersonRelationConfiguration : IEntityTypeConfiguration<PersonRelation>
    {
        public void Configure(EntityTypeBuilder<PersonRelation> builder)
        {
            builder.HasData(
                new PersonRelation
                {
                    RelatedFromId = 1,
                    RelatedToId = 2,
                    RelationType = "ნათესავი"
                });

            builder.HasData(
                new PersonRelation
                {
                    RelatedFromId = 2,
                    RelatedToId = 1,
                    RelationType = "ნათესავი"
                });

            builder.HasData(
                new PersonRelation
                {
                    RelatedFromId = 3,
                    RelatedToId = 1,
                    RelationType = "სხვა"
                });
        }
    }
}
