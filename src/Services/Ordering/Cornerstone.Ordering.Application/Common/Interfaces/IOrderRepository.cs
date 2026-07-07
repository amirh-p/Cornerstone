using Cornerstone.Ordering.Domain;

namespace Cornerstone.Ordering.Application.Common.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(OrderId id, CancellationToken ct);
    Task AddAsync(Order order, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}