using Cornerstone.Common.Mediator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Cornerstone.Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogApplication(this IServiceCollection services)
    {
        services.AddCornerstoneMediator(typeof(DependencyInjection).Assembly);
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
