using Cornerstone.Ordering.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cornerstone.Ordering.Infrastructure.Persistence.Configuration;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .HasConversion(id => id.Value, value => OrderId.From(value))
            .ValueGeneratedNever();

        builder.Property(o => o.CustomerId).IsRequired();
        builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(o => o.CreatedAt).IsRequired();

        builder.OwnsMany(o => o.Lines, lines =>
        {
            lines.ToTable("OrderLines");
            lines.WithOwner().HasForeignKey("OrderId");

            lines.Property<Guid>("LineId");
            lines.HasKey("LineId");

            lines.OwnsOne(l => l.Product, product =>
            {
                product.Property(p => p.ProductId).HasColumnName("ProductId");
                product.Property(p => p.ProductName).HasColumnName("ProductName").HasMaxLength(200);
            });

            lines.OwnsOne(l => l.UnitPrice, price =>
            {
                price.Property(m => m.Amount).HasColumnName("UnitPrice").HasPrecision(18, 2);
                price.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
            });

            lines.Property(l => l.Quantity).IsRequired();
        });

        builder.Navigation(o => o.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(o => o.DomainEvents);
        builder.Ignore(o => o.Total);
    }
}
