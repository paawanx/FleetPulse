using Microsoft.AspNetCore.Mvc;
using FleetPulse.Api.Dtos;

namespace FleetPulse.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VehiclesController : ControllerBase
{
    private List<VehicleResponseDto> _vehicles = new List<VehicleResponseDto>
    {
        new VehicleResponseDto { Id = 1, LicensePlate = "TRK-101", Status = "Active" },
        new VehicleResponseDto { Id = 2, LicensePlate = "TRK-102", Status = "Warning" }
    };

    [HttpGet]
    public IEnumerable<VehicleResponseDto> Get()
    {
        return [.._vehicles];
    }
}
