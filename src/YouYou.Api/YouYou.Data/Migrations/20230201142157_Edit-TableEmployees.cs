using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YouYou.Data.Migrations
{
    public partial class EditTableEmployees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GenderId",
                table: "Employees",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_GenderId",
                table: "Employees",
                column: "GenderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Genders_GenderId",
                table: "Employees",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Genders_GenderId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_GenderId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Employees");
        }
    }
}
