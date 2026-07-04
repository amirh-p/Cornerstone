using Cornerstone.Common.Mediator;
using Cornerstone.Catalog.Application.Products.Dtos;

namespace Cornerstone.Catalog.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;