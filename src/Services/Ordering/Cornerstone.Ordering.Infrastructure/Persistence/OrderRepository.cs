using Cornerstone.Ordering.Application.Common.Interfaces;
using Cornerstone.Ordering.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cornerstone.Ordering.Infrastructure.Persistence;

public sealed class OrderRepository(OrderingDbContext dbContext) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken ct)
    {
        await dbContext.AddAsync(order, ct);
    }

    public async Task<Order?> GetByIdAsync(OrderId id, CancellationToken ct)
    {
        return await dbContext.Orders.Include(o => o.Lines).FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}
