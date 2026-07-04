using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cornerstone.Catalog.Infrastructure.Persistence.Outbox;

namespace Cornerstone.Catalog.Infrastructure.Persistence.Configurations;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Type).HasMaxLength(500).IsRequired();
        builder.Property(m => m.Content).IsRequired();
        builder.HasIndex(m => m.ProcessedOn);
    }
}