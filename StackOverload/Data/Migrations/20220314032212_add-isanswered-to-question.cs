﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackOverload.Data.Migrations
{
    public partial class addisansweredtoquestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnswered",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnswered",
                table: "Questions");
        }
    }
}
