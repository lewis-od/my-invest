using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyInvest.Persistence;

namespace MyInvest.ComponentTests;

public class MyInvestApplicationFactory : WebApplicationFactory<Program>
{
    // TODO: Read from file
    private const string ConnectionString = "Host=localhost;Port=5433;Database=myinvest-tests;Username=myinvest-tests;Password=testingdb";

    private static readonly object Lock = new();
    private static bool _dbHasBeenInitialised = false;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            RemoveDbContextIfExists(services);
            RegisterTestDbContext(services);
            lock (Lock)
            {
                SetupDbForTests(services);
            }
        });
    }

    private static void RemoveDbContextIfExists(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MyInvestDbContext>));
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
    }

    private static void RegisterTestDbContext(IServiceCollection services)
    {
        services.AddDbContext<MyInvestDbContext>(options =>
        {
            options.UseNpgsql(ConnectionString);
        });
    }

    private static void SetupDbForTests(IServiceCollection services)
    {
        if (_dbHasBeenInitialised) return;
        
        var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MyInvestDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        _dbHasBeenInitialised = true;
    }

    protected override void ConfigureClient(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    }
}
