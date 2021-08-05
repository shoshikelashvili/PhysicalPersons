using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhysicalPersons.Migrations
{
    public partial class moreSampleData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "CityId", "Name" },
                values: new object[,]
                {
                    { 3, "Wyaltubo" },
                    { 4, "Toronto" },
                    { 5, "Tokyo" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Birthday", "CityId", "Gender", "Image", "LastName", "Name", "PersonalNumber" },
                values: new object[,]
                {
                    { 4, new DateTime(2001, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "ქალი", "/images/image1.jpg", "გოჩეჩილაძე", "მარიამ", "12345678910" },
                    { 5, new DateTime(2001, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "კაცი", "/images/image1.jpg", "Gelashvili", "Luka", "12345678910" }
                });

            migrationBuilder.InsertData(
                table: "PersonRelations",
                columns: new[] { "RelatedFromId", "RelatedToId", "RelationType" },
                values: new object[,]
                {
                    { 4, 5, "კოლეგა" },
                    { 5, 4, "კოლეგა" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Birthday", "CityId", "Gender", "Image", "LastName", "Name", "PersonalNumber" },
                values: new object[,]
                {
                    { 6, new DateTime(2001, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "კაცი", "/images/image1.jpg", "გაჩეჩილაძე", "Luka", "12345678910" },
                    { 7, new DateTime(2001, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "ქალი", "/images/image1.jpg", "გაფრინდაშვილი", "ნინო", "12345678910" }
                });

            migrationBuilder.InsertData(
                table: "PhoneNumbers",
                columns: new[] { "PhoneNumberId", "Number", "PersonId", "Type" },
                values: new object[] { 3, "58890309", 4, "სახლის" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PersonRelations",
                keyColumns: new[] { "RelatedFromId", "RelatedToId" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "PersonRelations",
                keyColumns: new[] { "RelatedFromId", "RelatedToId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "PhoneNumberId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "CityId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: 5);
        }
    }
}
