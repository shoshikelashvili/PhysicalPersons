using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasData(
                new Person
                {
                    Id = 1,
                    Name = "Rati",
                    LastName = "Shoshikelashvili",
                    Gender = "კაცი",
                    PersonalNumber = "12345678910",
                    Birthday = new DateTime(1999, 9, 11),
                });
        }
    }
}
