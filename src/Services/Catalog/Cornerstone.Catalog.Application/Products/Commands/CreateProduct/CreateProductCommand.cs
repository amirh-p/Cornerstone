using Cornerstone.Common.Mediator;
using Cornerstone.Catalog.Application.Products.Dtos;

namespace Cornerstone.Catalog.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(string Name, string Sku, decimal Price, string Currency)
    : IRequest<ProductDto>;