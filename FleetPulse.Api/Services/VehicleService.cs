using FleetPulse.Api.Dtos;
using FleetPulse.Api.Interfaces;
using FleetPulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetPulse.Api.Services;

public class VehicleService : IVehicleService
{
    private readonly AppDbContext _dbContext;
    public VehicleService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<VehicleResponseDto>> GetAllVehiclesAsync()
    {
        var vehicles = await _dbContext.Vehicles.ToListAsync();
        return vehicles.Select(v => new VehicleResponseDto
        {
            Id = v.Id,
            LicensePlate = v.LicensePlate,
            Status = v.Status
        });
    }

    public async Task<VehicleResponseDto> GetVehicleByIdAsync(int id)
    {
        var vehicle = await _dbContext.Vehicles.FindAsync(id);
        if (vehicle == null)
        {
            return null!;
        }

        return new VehicleResponseDto
        {
            Id = vehicle.Id,
            LicensePlate = vehicle.LicensePlate,
            Status = vehicle.Status
        };
    }
}