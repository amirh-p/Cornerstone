using Cornerstone.Ordering.Application.Common.Interfaces;
using Cornerstone.Ordering.Infrastructure.ExternalServices;
using Cornerstone.Ordering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cornerstone.Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderingInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderingDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("OrderingDb")));

        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:CatalogApi"]!);
        })
        .AddStandardResilienceHandler();

        return services;
    }
}
