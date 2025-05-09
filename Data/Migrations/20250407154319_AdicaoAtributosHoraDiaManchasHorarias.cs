using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoAtributosHoraDiaManchasHorarias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Dia",
                table: "ManchasHorarias",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraInicio",
                table: "ManchasHorarias",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dia",
                table: "ManchasHorarias");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "ManchasHorarias");
        }
    }
}
