using Microsoft.EntityFrameworkCore;
using FleetPulse.Infrastructure.Data;
using FleetPulse.Api.Interfaces;
using FleetPulse.Api.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ITelemetryService, TelemetryService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(
        connectionString,
        b => b.MigrationsAssembly("FleetPulse.Infrastructure")
    )
);

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(
            name: myAllowSpecificOrigins,
            policy =>
            {
                policy.WithOrigins(["http://localhost:5173","http://d1331wv9mzcvfl.cloudfront.net"])
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }
        );
    }
);

if (builder.Configuration.GetValue<bool>("RabbitMq:Enabled"))
{
    builder.Services.AddMassTransit(x =>
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(
                builder.Configuration["RabbitMq:Host"] ?? "localhost",
                "/",
                h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
        });
    });
}

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            DbInitializer.Seed(context);
        }catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }

    app.MapOpenApi();
    app.UseSwaggerUi(
        options =>
        {
            options.DocumentPath = "/openapi/v1.json";
        }
    );
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
