using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetPulse.Api.Migrations
{
    /// <inheritdoc />
    public partial class PrimaryKeyInVehicleAndForeignInTelemetry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Telemetries_VehicleId",
                table: "Telemetries",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Telemetries_Vehicles_VehicleId",
                table: "Telemetries",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Telemetries_Vehicles_VehicleId",
                table: "Telemetries");

            migrationBuilder.DropIndex(
                name: "IX_Telemetries_VehicleId",
                table: "Telemetries");
        }
    }
}
