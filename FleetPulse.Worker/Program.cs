using FleetPulse.Worker;
using FleetPulse.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "No database connection string was found. Add ConnectionStrings:DefaultConnection to the Worker configuration.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        connectionString,
        npgsql => npgsql.MigrationsAssembly("FleetPulse.Infrastructure")));

builder.Services.AddMassTransit(x => {
    x.AddConsumer<TelemetryConsumer>();

    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(builder.Configuration["RabbitMq:Host"] ?? "localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

app.Run();