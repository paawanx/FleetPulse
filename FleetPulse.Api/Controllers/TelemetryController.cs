using Microsoft.AspNetCore.Mvc;
using FleetPulse.Api.Dtos;
using FleetPulse.Api.Interfaces;

namespace FleetPulse.Api.Controllers;

[ApiController]
public class TelemetryController : ControllerBase
{

    private readonly ITelemetryService _telemetryService;

    public TelemetryController(ITelemetryService telemetryService)
    {
        _telemetryService = telemetryService;
    } 

    [HttpPost("Vehicles/telemetry")]
    public async Task<IActionResult> PostTelemetry([FromBody] VehicleTelemetryRequestDto requestDto)
    {
        await _telemetryService.ProcessTelemetryAsync(requestDto);
        return Accepted("Telemetry data received and is being processed.");
    }

    [HttpGet("Vehicles/{vehicleId:int}/telemetry")]
    public async Task<IActionResult> GetTelemetryByVehicleId([FromRoute(Name = "vehicleId")] int vehicleId)
    {
        var telemetryData = await _telemetryService.GetTelemetryByVehicleIdAsync(vehicleId);

        if (telemetryData is null || !telemetryData.Any())
        {
            return NotFound($"No telemetry data found for vehicle with ID {vehicleId}.");
        }

        return Ok(telemetryData);
    }
}