using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyInvest.Persistence;

// Used by EF at design-time (when creating and running migrations)
public class MyInvestDbContextFactory : IDesignTimeDbContextFactory<MyInvestDbContext>
{
    private const string ConfigFileLocation = "../MyInvest/appsettings.Development.json";

    public MyInvestDbContext CreateDbContext(string[] args)
    {
        var connectionString = ReadConnectionString();

        var optionsBuilder = new DbContextOptionsBuilder<MyInvestDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new MyInvestDbContext(optionsBuilder.Options);
    }

    private string ReadConnectionString()
    {
        using var stream = File.OpenRead(ConfigFileLocation);
        var config = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(stream);
        if (config == null)
        {
            throw new Exception($"{ConfigFileLocation} doesn't exist or contains invalid JSON");
        }

        var connectionString = config["ConnectionStrings"].GetProperty("Postgres").GetString();
        return connectionString ?? throw new Exception($"ConnectionStrings.Postgres not found in config file {ConfigFileLocation}");
    }
}
