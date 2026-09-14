using System.ComponentModel.DataAnnotations;

namespace FleetPulse.Infrastructure.Entities;
public class Vehicle
{
    [Key]
    public int Id { get; set; }
    public string LicensePlate { get; set; } = null!;
    public string DriverName { get; set; } = null!;
    public string Status { get; set; } = "Pending";
    public double LastLongitude { get; set; } = 0;
    public double LastLatitude { get; set; } = 0;
    public DateTime LastUpdated { get; set; }
}