namespace Cornerstone.Ordering.Application.Common.Interfaces;

public interface IProductCatalogClient
{
    Task<ProductSnapshot?> GetProductAsync(Guid productId, CancellationToken ct);
}

public sealed record ProductSnapshot(
    Guid ProductId,
    string Name,
    decimal Price,
    string Currency);
