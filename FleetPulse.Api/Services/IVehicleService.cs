using FleetPulse.Api.Dtos;

namespace FleetPulse.Api.Interfaces;

public interface IVehicleService
{
    Task<IEnumerable<VehicleResponseDto>> GetAllVehiclesAsync();
    Task<VehicleResponseDto> GetVehicleByIdAsync(int id);
}