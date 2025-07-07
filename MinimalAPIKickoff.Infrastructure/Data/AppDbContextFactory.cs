using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MinimalAPIKickoff.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<MinimalAPIKickoffContext>
{
    public MinimalAPIKickoffContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "MinimalAPIKickoff.Api"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<MinimalAPIKickoffContext>();

        optionsBuilder.UseSqlServer(connectionString);

        return new MinimalAPIKickoffContext(optionsBuilder.Options);
    }
}
