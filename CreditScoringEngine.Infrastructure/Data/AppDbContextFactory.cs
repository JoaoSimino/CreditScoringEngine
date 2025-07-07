using CreditScoringEngine.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MinimalAPIKickoff.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<CreditScoringEngineContext>
{
    public CreditScoringEngineContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "CreditScoringEngine.Api"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<CreditScoringEngineContext>();

        optionsBuilder.UseSqlServer(connectionString);

        return new CreditScoringEngineContext(optionsBuilder.Options);
    }
}
