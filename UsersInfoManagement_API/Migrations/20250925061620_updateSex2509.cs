﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersInfoManagement_API.Migrations
{
    /// <inheritdoc />
    public partial class updateSex2509 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "UsersInfo",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "UsersInfo");
        }
    }
}
