using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetPulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAlterTelemetryColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EngineTemperatureInCelsius",
                table: "Telemetries",
                newName: "Longitude");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Telemetries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Telemetries");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Telemetries",
                newName: "EngineTemperatureInCelsius");
        }
    }
}
