using FleetPulse.Worker;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransit(x => {
        x.AddConsumer<TelemetryConsumer>();

        x.UsingRabbitMq((context, cfg) => {
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
        });
});

var host = builder.Build();
host.Run();
