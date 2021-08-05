using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasData(
                new City
                {
                    Id = 1,
                    Name = "Tbilisi"
                });

            builder.HasData(
                new City
                {
                    Id = 2,
                    Name="Batumi",
                });
            builder.HasData(
                new City
                {
                    Id = 3,
                    Name = "Wyaltubo",
                });
            builder.HasData(
                new City
                {
                    Id = 4,
                    Name = "Toronto",
                });
            builder.HasData(
                new City
                {
                    Id = 5,
                    Name = "Tokyo",
                });
        }
    }
}
