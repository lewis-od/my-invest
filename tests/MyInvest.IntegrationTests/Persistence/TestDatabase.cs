using Microsoft.EntityFrameworkCore;
using MyInvest.Persistence;

namespace MyInvest.IntegrationTests.Persistence;

public class TestDatabase
{
    private const int DbPort = 5433;
    private const string DbName = "myinvest-tests";
    private const string DbUsername = "myinvest-tests";
    private const string DbPassword = "testingdb";
    private static readonly string ConnectionString = $"Host=localhost;Port={DbPort};Database={DbName};Username={DbUsername};Password={DbPassword}";

    private static readonly object _lock = new();
    private static bool _databaseHasBeenInitialised;

    public TestDatabase()
    {
        lock (_lock)
        {
            if (_databaseHasBeenInitialised) return;

            using var context = CreateContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            _databaseHasBeenInitialised = true;
        }
    }

    public MyInvestDbContext CreateContext() => new(new DbContextOptionsBuilder().UseNpgsql(ConnectionString).Options);
}
