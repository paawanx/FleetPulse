using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.Json;

namespace FleetPulse.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Navigate up from FleetPulse.Infrastructure to locate FleetPulse.Api
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../FleetPulse.Api");

        var connectionString = ReadConnectionString(Path.Combine(basePath, "appsettings.json"));
        var developmentPath = Path.Combine(basePath, "appsettings.Development.json");
        if (File.Exists(developmentPath))
        {
            connectionString = ReadConnectionString(developmentPath) ?? connectionString;
        }

        connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? connectionString;

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "No database connection string was found. Set ConnectionStrings__DefaultConnection or add it to FleetPulse.Api/appsettings.Development.json.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            options => options.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name));

        return new AppDbContext(optionsBuilder.Options);
    }

    private static string? ReadConnectionString(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        using var document = JsonDocument.Parse(File.ReadAllText(path));
        if (!document.RootElement.TryGetProperty("ConnectionStrings", out var connectionStrings)
            || !connectionStrings.TryGetProperty("DefaultConnection", out var defaultConnection))
        {
            return null;
        }

        return defaultConnection.GetString();
    }
}