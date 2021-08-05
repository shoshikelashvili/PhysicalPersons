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
                    CityId = 1,
                    Image = "/images/image1.jpg"
                });

            builder.HasData(
                new Person
                {
                    Id = 2,
                    Name = "Giorgi",
                    LastName = "Axalkacishvili",
                    Gender = "კაცი",
                    PersonalNumber = "12345678910",
                    Birthday = new DateTime(1980, 9, 11),
                    CityId = 1,
                    Image = "/images/image1.jpg"
                });

            builder.HasData(
                new Person
                {
                    Id = 3,
                    Name = "მარიამ",
                    LastName = "გოჩეჩილაძე",
                    Gender = "ქალი",
                    PersonalNumber = "12345678910",
                    Birthday = new DateTime(2001, 9, 11),
                    CityId = 2,
                    Image = "/images/image1.jpg"
                });

            builder.HasData(
                new Person
                {
                    Id = 4,
                    Name = "მარიამ",
                    LastName = "გოჩეჩილაძე",
                    Gender = "ქალი",
                    PersonalNumber = "12345678910",
                    Birthday = new DateTime(2001, 9, 11),
                    CityId = 2,
                    Image = "/images/image1.jpg"
                });

            builder.HasData(
                new Person
                {
                    Id = 5,
                    Name = "Luka",
                    LastName = "Gelashvili",
                    Gender = "კაცი",
                    PersonalNumber = "12345678910",
                    Birthday = new DateTime(2001, 9, 11),
                    CityId = 2,
                    Image = "/images/image1.jpg"
                });

            builder.HasData(
                new Person
                {
                    Id = 6,
                    Name = "Luka",
                    LastName = "გაჩეჩილაძე",
                    Gender = "კაცი",
                    PersonalNumber = "12345678910",
                    Birthday = new DateTime(2001, 9, 11),
                    CityId = 4,
                    Image = "/images/image1.jpg"
                });

            builder.HasData(
                new Person
                {
                    Id = 7,
                    Name = "ნინო",
                    LastName = "გაფრინდაშვილი",
                    Gender = "ქალი",
                    PersonalNumber = "12345678910",
                    Birthday = new DateTime(2001, 9, 11),
                    CityId = 5,
                    Image = "/images/image1.jpg"
                });
        }
    }
}
