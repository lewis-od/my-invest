using Microsoft.EntityFrameworkCore;
using MyInvest.Persistence;

namespace MyInvest.IntegrationTests.Persistence;

public class TestDatabase
{
    private const string ConnectionString = "Host=localhost;Port=5432;Database=myinvest;Username=myinvest;Password=topsecret";

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

    public MyInvestDbContext CreateContext() => new (new DbContextOptionsBuilder().UseNpgsql(ConnectionString).Options);
}
