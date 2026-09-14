using Microsoft.EntityFrameworkCore;
using FleetPulse.Infrastructure.Entities;

namespace FleetPulse.Infrastructure.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {}

    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    public DbSet<Telemetry> Telemetries { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vehicle>().ToTable("Vehicles");
        modelBuilder.Entity<Telemetry>().ToTable("Telemetries");
    }
}