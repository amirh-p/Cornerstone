using Cornerstone.Common;

namespace Cornerstone.Ordering.Domain.Events;

public sealed record OrderCancelled(OrderId OrderId, string Reason, DateTime OccurredOn) : IDomainEvent;