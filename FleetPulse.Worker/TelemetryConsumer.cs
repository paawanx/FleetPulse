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
        try{
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

            _logger.LogInformation("Telemetry data saved to database for VehicleId: {VehicleId}", telemetry.VehicleId);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error processing telemetry message: {Message}", context.Message);
            throw; // Optionally rethrow to let MassTransit handle the failure (e.g., retry, dead-letter)
        }finally
        {
            _logger.LogInformation("Finished processing telemetry message for VehicleId: {VehicleId}", context.Message.VehicleId);
        }
    }
}
