using MassTransit;
using FleetPulse.Contracts;

namespace FleetPulse.Worker;

public class TelemetryConsumer : IConsumer<VehicleTelemetryEvent>
{
    private readonly ILogger<TelemetryConsumer> logger;

    public TelemetryConsumer(ILogger<TelemetryConsumer> logger)
    {
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<VehicleTelemetryEvent> context)
    {
        var telemetry = context.Message;
        logger.LogInformation("Received telemetry for VehicleId: {VehicleId}, Latitude: {Latitude}, Longitude: {Longitude}, Speed: {Speed}, FuelLevel: {FuelLevel}, Timestamp: {Timestamp}",
            telemetry.VehicleId, telemetry.Latitude, telemetry.Longitude, telemetry.Speed, telemetry.FuelLevel, telemetry.Timestamp);

        // TODO: Save to PostgreSQL using AppDbContext here
        return Task.CompletedTask;
    }
}
