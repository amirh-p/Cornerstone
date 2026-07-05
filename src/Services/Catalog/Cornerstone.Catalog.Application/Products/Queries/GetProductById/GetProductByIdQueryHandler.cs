using Cornerstone.Catalog.Application.Common.Exceptions;
using Cornerstone.Catalog.Application.Common.Interfaces;
using Cornerstone.Catalog.Application.Products.Dtos;
using Cornerstone.Catalog.Domain;
using Cornerstone.Common.Mediator;

namespace Cornerstone.Catalog.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler(IProductRepository repository)
    : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await repository.GetByIdAsync(ProductId.From(request.Id), ct)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        return new ProductDto(
            product.Id.Value,
            product.Name,
            product.Sku,
            product.Price.Amount,
            product.Price.Currency,
            product.IsActive);
    }
}