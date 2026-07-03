using Cornerstone.Common;

namespace Cornerstone.Catalog.Domain.Events;

public sealed record ProductCreated(
    ProductId ProductId,
    string Name,
    DateTime OccurredOn) : IDomainEvent;