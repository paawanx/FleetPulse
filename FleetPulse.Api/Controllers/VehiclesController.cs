using Microsoft.AspNetCore.Mvc;
using FleetPulse.Api.Models;

namespace FleetPulse.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VehiclesController : ControllerBase
{
    private List<Vehicle> _vehicles = new List<Vehicle>
    {
        new Vehicle { Id = 1, LicensePlate = "TRK-101", Status = "Active" },
        new Vehicle { Id = 2, LicensePlate = "TRK-102", Status = "Warning" }
    };

    [HttpGet]
    public IEnumerable<Vehicle> Get()
    {
        return [.._vehicles];
    }
}
