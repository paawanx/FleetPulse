using MassTransit;
using FleetPulse.Contracts;
using FleetPulse.Infrastructure.Data;
using FleetPulse.Infrastructure.Entities;

namespace FleetPulse.Worker;

public class TelemetryConsumer : IConsumer<VehicleTelemetryEvent>
{
    private readonly ILogger<TelemetryConsumer> _logger;
    private readonly AppDbContext _dbContext;

    public TelemetryConsumer(ILogger<TelemetryConsumer> logger, AppDbContext dbContext)
    {
        this._logger = logger;
        this._dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<VehicleTelemetryEvent> context)
    {
        var telemetry = context.Message;
        _logger.LogInformation("Received telemetry for VehicleId: {VehicleId}, Latitude: {Latitude}, Longitude: {Longitude}, Speed: {Speed}, FuelLevel: {FuelLevel}, Timestamp: {Timestamp}",
            telemetry.VehicleId, telemetry.Latitude, telemetry.Longitude, telemetry.Speed, telemetry.FuelLevel, telemetry.Timestamp);

        _dbContext.Telemetries.Add(new Telemetry
        {
            VehicleId = telemetry.VehicleId,
            Latitude = telemetry.Latitude,
            Longitude = telemetry.Longitude,
            SpeedInKmh = telemetry.Speed,
            FuelLevelInPercentage = telemetry.FuelLevel,
            Timestamp = telemetry.Timestamp
        });

        await _dbContext.SaveChangesAsync();
    }
}
