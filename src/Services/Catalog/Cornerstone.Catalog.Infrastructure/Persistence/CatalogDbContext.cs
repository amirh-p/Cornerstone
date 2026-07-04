// Persistence/CatalogDbContext.cs
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Cornerstone.Common;
using Cornerstone.Catalog.Domain;
using Cornerstone.Catalog.Infrastructure.Persistence.Outbox;

namespace Cornerstone.Catalog.Infrastructure.Persistence;

public sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDomainEventsToOutboxMessages();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ConvertDomainEventsToOutboxMessages()
    {
        var aggregatesWithEvents = ChangeTracker.Entries<IAggregateRoot>()
            .Select(entry => entry.Entity)
            .Where(aggregate => aggregate.DomainEvents.Count != 0)
            .ToList();

        var outboxMessages = aggregatesWithEvents
            .SelectMany(aggregate => aggregate.DomainEvents)
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                OccurredOn = domainEvent.OccurredOn
            });

        OutboxMessages.AddRange(outboxMessages);

        foreach (var aggregate in aggregatesWithEvents)
            aggregate.ClearDomainEvents();
    }
}