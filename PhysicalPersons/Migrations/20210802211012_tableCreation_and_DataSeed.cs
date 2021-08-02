using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhysicalPersons.Migrations
{
    public partial class tableCreation_and_DataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PersonalNumber = table.Column<string>(type: "char(11)", maxLength: 11, nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_Persons_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonRelations",
                columns: table => new
                {
                    RelatedFromId = table.Column<int>(type: "int", nullable: false),
                    RelatedToId = table.Column<int>(type: "int", nullable: false),
                    RelationType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRelations", x => new { x.RelatedFromId, x.RelatedToId });
                    table.ForeignKey(
                        name: "FK_PersonRelations_Persons_RelatedFromId",
                        column: x => x.RelatedFromId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonRelations_Persons_RelatedToId",
                        column: x => x.RelatedToId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumbers",
                columns: table => new
                {
                    PhoneNumberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumbers", x => x.PhoneNumberId);
                    table.ForeignKey(
                        name: "FK_PhoneNumbers_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "CityId", "Name" },
                values: new object[] { 1, "Tbilisi" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "CityId", "Name" },
                values: new object[] { 2, "Batumi" });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Birthday", "CityId", "Gender", "Image", "LastName", "Name", "PersonalNumber" },
                values: new object[] { 1, new DateTime(1999, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "კაცი", "/images/image1.jpg", "Shoshikelashvili", "Rati", "12345678910" });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Birthday", "CityId", "Gender", "Image", "LastName", "Name", "PersonalNumber" },
                values: new object[] { 2, new DateTime(1980, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "კაცი", "/images/image1.jpg", "Axalkacishvili", "Giorgi", "12345678910" });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Birthday", "CityId", "Gender", "Image", "LastName", "Name", "PersonalNumber" },
                values: new object[] { 3, new DateTime(2001, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "ქალი", "/images/image1.jpg", "გოჩეჩილაძე", "მარიამ", "12345678910" });

            migrationBuilder.InsertData(
                table: "PersonRelations",
                columns: new[] { "RelatedFromId", "RelatedToId", "RelationType" },
                values: new object[,]
                {
                    { 1, 2, "ნათესავი" },
                    { 2, 1, "ნათესავი" },
                    { 3, 1, "სხვა" }
                });

            migrationBuilder.InsertData(
                table: "PhoneNumbers",
                columns: new[] { "PhoneNumberId", "Number", "PersonId", "Type" },
                values: new object[,]
                {
                    { 1, "555940789", 1, "მობილური" },
                    { 2, "58890309", 2, "სახლის" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonRelations_RelatedToId",
                table: "PersonRelations",
                column: "RelatedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CityId",
                table: "Persons",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumbers_PersonId",
                table: "PhoneNumbers",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonRelations");

            migrationBuilder.DropTable(
                name: "PhoneNumbers");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
