using Microsoft.AspNetCore.Mvc;
using FleetPulse.Api.Dtos;
using FleetPulse.Api.Interfaces;
using FleetPulse.Contracts;
using MassTransit;

namespace FleetPulse.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VehiclesController : ControllerBase
{

    private readonly IVehicleService _vehicleService;
    private readonly IPublishEndpoint _publishEndpoint;

    public VehiclesController(IVehicleService vehicleService, IPublishEndpoint publishEndpoint)
    {
        _vehicleService = vehicleService;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<IEnumerable<VehicleResponseDto>> Get()
    {
        return await _vehicleService.GetAllVehiclesAsync();
    }

    [HttpPost("telemetry")]
    public async Task<IActionResult> PostTelemetry([FromBody] VehicleTelemetryRequestDto requestDto)
    {
        var telemetryEvent = new VehicleTelemetryEvent(
            requestDto.VehicleId,
            requestDto.Latitude,
            requestDto.Longitude,
            requestDto.Speed,
            requestDto.FuelLevel,
            DateTime.UtcNow
        );

        await _publishEndpoint.Publish(telemetryEvent);

        return Accepted("Telemetry data received and is being processed.");
    }
}
