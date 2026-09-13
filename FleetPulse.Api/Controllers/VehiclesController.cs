using Microsoft.AspNetCore.Mvc;
using FleetPulse.Api.Dtos;
using FleetPulse.Api.Interfaces;

namespace FleetPulse.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VehiclesController : ControllerBase
{

    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    private List<VehicleResponseDto> _vehicles = new List<VehicleResponseDto>
    {
        new VehicleResponseDto { Id = 1, LicensePlate = "TRK-101", Status = "Active" },
        new VehicleResponseDto { Id = 2, LicensePlate = "TRK-102", Status = "Warning" }
    };

    [HttpGet]
    public async Task<IEnumerable<VehicleResponseDto>> Get()
    {
        return await _vehicleService.GetAllVehiclesAsync();
    }
}
