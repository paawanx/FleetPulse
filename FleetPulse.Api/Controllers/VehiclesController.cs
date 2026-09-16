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

    [HttpGet]
    public async Task<IEnumerable<VehicleResponseDto>> Get()
    {
        return await _vehicleService.GetAllVehiclesAsync();
    }
}
