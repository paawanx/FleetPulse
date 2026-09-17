using FleetPulse.Api.Dtos;

namespace FleetPulse.Api.Interfaces;

public interface ITelemetryService
{
    Task ProcessTelemetryAsync(VehicleTelemetryRequestDto telemetryRequest);
    Task<IEnumerable<VehicleTelemetryResponseDto>> GetTelemetryByVehicleIdAsync(int vehicleId);
}