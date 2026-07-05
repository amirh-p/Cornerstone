using Cornerstone.Catalog.Application.Products.Dtos;
using Cornerstone.Common.Mediator;

namespace Cornerstone.Catalog.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(string Name, string Sku, decimal Price, string Currency)
    : IRequest<ProductDto>;