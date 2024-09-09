using TechTest.Data;
using TechTest.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

var recreateDb = builder.Configuration.GetValue<bool>("DELETE_AND_RECREATE_DB");
builder.Services.AddHostedService<Worker>(provider =>
    new Worker(provider, provider.GetService<IHostApplicationLifetime>(), recreateDb));

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddSqlServerDbContext<TechTestDbContext>("TechTest");

var host = builder.Build();
host.Run();
