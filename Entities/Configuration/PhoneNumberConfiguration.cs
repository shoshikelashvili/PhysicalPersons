using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
    {
        public void Configure(EntityTypeBuilder<PhoneNumber> builder)
        {
            builder.HasData(
                new PhoneNumber
                {
                    Id = 1,
                    Type = "მობილური",
                    Number = "555940789",
                    PersonId = 1, 
                });

            builder.HasData(
                new PhoneNumber
                {
                    Id = 2,
                    Type = "სახლის",
                    Number = "58890309",
                    PersonId = 2
                });

            builder.HasData(
                new PhoneNumber
                {
                    Id = 3,
                    Type = "სახლის",
                    Number = "58890309",
                    PersonId = 4
                });
        }
    }
}
