using Cornerstone.Common.Mediator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Cornerstone.Ordering.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddOrderingApplication()
        {
            services.AddCornerstoneMediator(typeof(DependencyInjection).Assembly);
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
