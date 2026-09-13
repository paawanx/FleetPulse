namespace FleetPulse.Api.Entities;
public class Vehicle
{
    public int Id { get; set; }
    public string LicensePlate { get; set; }
    public string DriverName { get; set; }
    public string Status { get; set; }
    public string LastLongitude { get; set; }
    public string LastLatitude { get; set; }
    public DateTime LastUpdated { get; set; }
}