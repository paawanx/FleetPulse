using System.ComponentModel.DataAnnotations;

namespace FleetPulse.Api.Entities;
public class Vehicle
{
    [Key]
    public int Id { get; set; }
    public string LicensePlate { get; set; } = null!;
    public string DriverName { get; set; } = null!;
    public string Status { get; set; } = "Pending";
    public string LastLongitude { get; set; } = string.Empty;
    public string LastLatitude { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
}