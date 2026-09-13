using FleetPulse.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FleetPulse.Api.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.Migrate();

            if (context.Vehicles.Any())
            {
                return; // DB has been seeded already
            }

            var vehicles = new Vehicle[]
            {
                new Vehicle { Id = 1, LicensePlate = "TRK-101", DriverName = "John Doe", Status = "Active" },
                new Vehicle { Id = 2, LicensePlate = "TRK-102", DriverName = "Jane Smith", Status = "Warning" }
            };

            context.Vehicles.AddRange(vehicles);
            context.SaveChanges();
        }
    }
}