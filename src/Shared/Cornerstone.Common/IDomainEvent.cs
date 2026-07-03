namespace Cornerstone.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
