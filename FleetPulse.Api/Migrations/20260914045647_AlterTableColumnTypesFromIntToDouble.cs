using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetPulse.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableColumnTypesFromIntToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Handle empty strings (""), spaces, or invalid text safely during conversion
            migrationBuilder.Sql(@"
                ALTER TABLE ""Vehicles"" 
                ALTER COLUMN ""LastLongitude"" TYPE double precision 
                USING (
                    CASE 
                        WHEN trim(""LastLongitude"") = '' THEN 0 
                        ELSE ""LastLongitude""::double precision 
                    END
                );
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Vehicles"" 
                ALTER COLUMN ""LastLatitude"" TYPE double precision 
                USING (
                    CASE 
                        WHEN trim(""LastLatitude"") = '' THEN 0 
                        ELSE ""LastLatitude""::double precision 
                    END
                );
            ");

            // Keep the rest of your integer -> double precision conversions
            migrationBuilder.AlterColumn<double>(
                name: "SpeedInKmh",
                table: "Telemetries",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "FuelLevelInPercentage",
                table: "Telemetries",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "EngineTemperatureInCelsius",
                table: "Telemetries",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Vehicles\" ALTER COLUMN \"LastLongitude\" TYPE text USING \"LastLongitude\"::text;");
            migrationBuilder.Sql("ALTER TABLE \"Vehicles\" ALTER COLUMN \"LastLatitude\" TYPE text USING \"LastLatitude\"::text;");

            migrationBuilder.AlterColumn<int>(
                name: "SpeedInKmh",
                table: "Telemetries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "FuelLevelInPercentage",
                table: "Telemetries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "EngineTemperatureInCelsius",
                table: "Telemetries",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}