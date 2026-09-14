namespace FleetPulse.Contracts;

public record VehicleTelemetryEvent
(
    int VehicleId,
    double Latitude,
    double Longitude,
    double Speed,
    double FuelLevel,
    DateTime Timestamp
);