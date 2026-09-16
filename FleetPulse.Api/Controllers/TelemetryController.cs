using Microsoft.AspNetCore.Mvc;
using FleetPulse.Api.Dtos;
using FleetPulse.Contracts;
using MassTransit;

namespace FleetPulse.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TelemetryController : ControllerBase
{

    private readonly IPublishEndpoint _publishEndpoint;

    public TelemetryController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost("vehicle/telemetry")]
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
