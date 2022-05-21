using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyInvest.Persistence;

// Used by EF at design-time (when creating and running migrations)
public class MyInvestDbContextFactory : IDesignTimeDbContextFactory<MyInvestDbContext>
{
    private const string ConnectionStringEnvVar = "ConnectionStrings__Postgres";
    private const string ConfigFileLocation = "../MyInvest/appsettings.Development.json";

    public MyInvestDbContext CreateDbContext(string[] args)
    {
        var connectionString = TryReadConnectionString();

        var optionsBuilder = new DbContextOptionsBuilder<MyInvestDbContext>();
        if (connectionString != null)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
        else
        {
            optionsBuilder.UseNpgsql();
        }

        return new MyInvestDbContext(optionsBuilder.Options);
    }

    private static string? TryReadConnectionString() => TryReadConnectionStringFromEnvVar() ?? TryReadConnectionStringFromConfigFile();

    private static string? TryReadConnectionStringFromEnvVar() => Environment.GetEnvironmentVariable(ConnectionStringEnvVar);

    private static string? TryReadConnectionStringFromConfigFile()
    {
        if (!File.Exists(ConfigFileLocation))
        {
            return null;
        }
        
        using var stream = File.OpenRead(ConfigFileLocation);
        var config = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(stream);
        if (config == null)
        {
            throw new Exception($"{ConfigFileLocation} contains invalid JSON");
        }

        var connectionString = config["ConnectionStrings"].GetProperty("Postgres").GetString();
        return connectionString ?? throw new Exception($"ConnectionStrings.Postgres not found in config file {ConfigFileLocation}");
    }
}
