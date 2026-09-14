using System.ComponentModel.DataAnnotations;

namespace FleetPulse.Api.Entities;

public class Telemetry
{
    [Key]
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public double SpeedInKmh { get; set; }
    public double EngineTemperatureInCelsius { get; set; }
    public double FuelLevelInPercentage { get; set; }
    public DateTime Timestamp { get; set; }

    public virtual Vehicle Vehicle { get; set; } = null!;
}