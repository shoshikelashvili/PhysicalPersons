using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhysicalPersons.Migrations
{
    public partial class field_fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Persons",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Birthday", "CityId", "Gender", "Image", "LastName", "Name", "PersonalNumber" },
                values: new object[] { 1, new DateTime(1999, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "კაცი", null, "Shoshikelashvili", "Rati", "12345678910" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Persons",
                type: "char(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);
        }
    }
}
