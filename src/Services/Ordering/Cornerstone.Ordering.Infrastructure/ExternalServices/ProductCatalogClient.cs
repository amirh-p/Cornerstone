using Cornerstone.Ordering.Application.Common.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace Cornerstone.Ordering.Infrastructure.ExternalServices;

public sealed class ProductCatalogClient(HttpClient httpClient) : IProductCatalogClient
{
    public async Task<ProductSnapshot?> GetProductAsync(Guid productId, CancellationToken ct)
    {
        var response = await httpClient.GetAsync($"/api/products/{productId}", ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<CatalogProductResponse>(ct);

        return dto is null ? null : new ProductSnapshot(dto.Id, dto.Name, dto.Price, dto.Currency);
    }

    private sealed record CatalogProductResponse(
        Guid Id,
        string Name,
        string Sku,
        decimal Price,
        string Currency,
        bool IsActive);
}
