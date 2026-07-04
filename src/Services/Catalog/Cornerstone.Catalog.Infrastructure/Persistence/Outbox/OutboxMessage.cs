namespace Cornerstone.Catalog.Infrastructure.Persistence.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; } = default!;
    public string Content { get; init; } = default!;
    public DateTime OccurredOn { get; init; }
    public DateTime? ProcessedOn { get; set; }
    public string? Error { get; set; }
}