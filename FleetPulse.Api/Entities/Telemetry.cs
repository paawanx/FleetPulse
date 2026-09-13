namespace FleetPulse.Api.Entities;

public class Telemetry
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int SpeedInKmh { get; set; }
    public int EngineTemperatureInCelsius { get; set; }
    public int FuelLevelInPercentage { get; set; }
    public DateTime Timestamp { get; set; }
}