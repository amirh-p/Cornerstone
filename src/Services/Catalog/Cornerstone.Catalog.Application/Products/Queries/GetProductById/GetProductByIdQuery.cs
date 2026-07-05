using Cornerstone.Catalog.Application.Products.Dtos;
using Cornerstone.Common.Mediator;

namespace Cornerstone.Catalog.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;