using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cornerstone.Catalog.Infrastructure.Persistence;

public sealed class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
{
    public CatalogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=cornerstone_catalog;Username=postgres;Password=postgres");
        return new CatalogDbContext(optionsBuilder.Options);
    }
}