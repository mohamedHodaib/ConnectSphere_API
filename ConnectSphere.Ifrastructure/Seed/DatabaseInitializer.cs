// Infrastructure/Seed/DatabaseInitializer.cs
using ConnectSphere.Application.Common.Interfaces;
using ConnectSphere.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace CleanArch.Infrastructure.Seed;

public class DatabaseInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly IIdentitySeeder _identitySeeder;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(
        ApplicationDbContext context,
        IIdentitySeeder identitySeeder,
        ILogger<DatabaseInitializer> logger)
    {
        _context = context;
        _identitySeeder = identitySeeder;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
            {
                _logger.LogInformation("Applying pending migrations...");
                await _context.Database.MigrateAsync();
                _logger.LogInformation("Migrations applied successfully");
            }

            await SeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }

    private async Task SeedAsync()
    {
        await _identitySeeder.SeedAsync();
        await _context.SaveChangesAsync();
    }

}