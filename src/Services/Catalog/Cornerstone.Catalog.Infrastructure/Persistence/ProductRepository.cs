using Microsoft.EntityFrameworkCore;
using Cornerstone.Catalog.Domain;
using Cornerstone.Catalog.Application.Common.Interfaces;

namespace Cornerstone.Catalog.Infrastructure.Persistence;

public sealed class ProductRepository(CatalogDbContext dbContext) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(ProductId id, CancellationToken ct) =>
        await dbContext.Products.FindAsync([id], cancellationToken: ct);

    public Task<Product?> GetBySkuAsync(string sku, CancellationToken ct) =>
        dbContext.Products.FirstOrDefaultAsync(p => p.Sku == sku, ct);

    public async Task AddAsync(Product product, CancellationToken ct) =>
        await dbContext.Products.AddAsync(product, ct);

    public Task SaveChangesAsync(CancellationToken ct) =>
        dbContext.SaveChangesAsync(ct);
}