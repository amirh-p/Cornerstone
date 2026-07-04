using Cornerstone.Common.Mediator;
using Cornerstone.Catalog.Domain;
using Cornerstone.Catalog.Application.Common.Interfaces;
using Cornerstone.Catalog.Application.Products.Dtos;

namespace Cornerstone.Catalog.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler(IProductRepository repository)
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var existing = await repository.GetBySkuAsync(request.Sku, ct);
        if (existing is not null)
            throw new InvalidOperationException($"A product with SKU '{request.Sku}' already exists.");

        var price = Money.Create(request.Price, request.Currency);
        var product = Product.Create(request.Name, request.Sku, price);

        await repository.AddAsync(product, ct);
        await repository.SaveChangesAsync(ct);

        return new ProductDto(
            product.Id.Value,
            product.Name,
            product.Sku,
            product.Price.Amount,
            product.Price.Currency,
            product.IsActive);
    }
}