using Microsoft.EntityFrameworkCore;
using MyInvest.Persistence.Clients;

namespace MyInvest.Persistence;

public class MyInvestDbContext : DbContext
{
    public DbSet<ClientEntity> Clients { get; set; } = null!;

    public MyInvestDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientEntity>();
    }
}
