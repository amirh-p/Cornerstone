using Cornerstone.Catalog.Application.Common.Interfaces;
using Cornerstone.Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cornerstone.Catalog.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCatalogInfrastructure(IConfiguration configuration)
        {
            services.AddDbContext<CatalogDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("CatalogDb")));

            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}