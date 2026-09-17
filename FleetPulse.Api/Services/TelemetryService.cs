using FleetPulse.Api.Dtos;
using FleetPulse.Api.Interfaces;
using FleetPulse.Infrastructure.Data;
using FleetPulse.Contracts;
using Microsoft.EntityFrameworkCore;
using MassTransit;

namespace FleetPulse.Api.Services;

public class TelemetryService : ITelemetryService
{
    private readonly AppDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    public TelemetryService(AppDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task ProcessTelemetryAsync(VehicleTelemetryRequestDto telemetryRequest)
    {
        var telemetryEvent = new VehicleTelemetryEvent(
            telemetryRequest.VehicleId,
            telemetryRequest.Latitude,
            telemetryRequest.Longitude,
            telemetryRequest.Speed,
            telemetryRequest.FuelLevel,
            DateTime.UtcNow
        );

        await _publishEndpoint.Publish(telemetryEvent);
    }

    public async Task<IEnumerable<VehicleTelemetryResponseDto>> GetTelemetryByVehicleIdAsync(int vehicleId)
    {
        var telemetryData = await _dbContext.Telemetries
            .Where(t => t.VehicleId == vehicleId)
            .OrderByDescending(t => t.Timestamp)
            .ToListAsync();

        return telemetryData.Select(t => new VehicleTelemetryResponseDto
        {
            VehicleId = t.VehicleId,
            Speed = t.SpeedInKmh,
            FuelLevel = t.FuelLevelInPercentage,
            Longitude = t.Longitude,
            Latitude = t.Latitude,
            Timestamp = t.Timestamp
        });
    }
}