using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Trace;
using TechTest.Data;
using TechTest.Data.Models;

namespace TechTest.MigrationService;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly bool _recreateDb;

    public Worker(IServiceProvider serviceProvider,
        IHostApplicationLifetime hostApplicationLifetime,
        bool recreateDb)
    {
        _serviceProvider = serviceProvider;
        _hostApplicationLifetime = hostApplicationLifetime;
        _recreateDb = recreateDb;
    }

    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource SActivitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = SActivitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TechTestDbContext>();

            var strategy = new SqlServerRetryingExecutionStrategy(
                dbContext,
                10,
                TimeSpan.FromSeconds(5),
                new[] {0});

            await strategy.ExecuteAsync(() => EnsureDatabaseAsync(dbContext, cancellationToken));
            await strategy.ExecuteAsync(() => RunMigrationAsync(dbContext, cancellationToken));
            await strategy.ExecuteAsync(() => SeedDataAsync(dbContext, cancellationToken));
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }

        _hostApplicationLifetime.StopApplication();
    }

    private async Task EnsureDatabaseAsync(TechTestDbContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        // check if environment variable is set to delete and recreate the database
        if (_recreateDb && await dbCreator.ExistsAsync(cancellationToken))
        {
            await dbCreator.DeleteAsync(cancellationToken);
        }

        // Create the database if it does not exist.
        // Do this first so there is then a database to start a transaction against.
        if (!await dbCreator.ExistsAsync(cancellationToken))
        {
            await dbCreator.CreateAsync(cancellationToken);
        }
    }

    private static async Task RunMigrationAsync(TechTestDbContext dbContext, CancellationToken cancellationToken)
    {
        // Run migration in a transaction to avoid partial migration if it fails.
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        await dbContext.Database.MigrateAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    private async Task SeedDataAsync(TechTestDbContext dbContext, CancellationToken cancellationToken)
    {
        if (!_recreateDb)
        {
            return;
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var categories = new List<Category>
        {
            new() {Name = "Accessories"},
            new() {Name = "Clothes"},
            new() {Name = "Food"},
            new() {Name = "Electronics"},
        };
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();

        var dbCategories = await dbContext.Categories.ToListAsync(cancellationToken);

        var products = new List<Product>();
        var random = new Random();

        var year = DateTime.UtcNow.Subtract(TimeSpan.FromDays(360));
        var halfYear = DateTime.UtcNow.Subtract(TimeSpan.FromDays(170));
        var week = DateTime.UtcNow.Subtract(TimeSpan.FromDays(5));
        var dates = new List<DateTime> {year, halfYear, week};

        foreach (var category in dbCategories)
        {
            var productCount = random.Next(5, 50);
            for (var i = 1; i <= productCount; i++)
            {
                var product = new Product
                {
                    Name = $"Product {i}-{i}",
                    Price = random.Next(1, 1001),
                    Category = category,
                    Sku = $"SKU-{i}-{i}",
                    Quantity = random.Next(1, 100),
                    DateAdded = dates.ElementAt(random.Next(0, dates.Count)),
                };
                products.Add(product);
            }
        }

        dbContext.Products.AddRange(products);
        await dbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}
