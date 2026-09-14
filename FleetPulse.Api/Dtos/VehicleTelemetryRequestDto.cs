namespace FleetPulse.Api.Dtos;

public class VehicleTelemetryRequestDto
{
    public int Id { get; set; }
    public double Speed { get; set; }
    public double FuelLevel { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public DateTime Timestamp { get; set; }
}