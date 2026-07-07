using Cornerstone.Common;

namespace Cornerstone.Order.Domain.Events;

public sealed record OrderCancelled(OrderId OrderId, string Reason, DateTime OccurredOn) : IDomainEvent;