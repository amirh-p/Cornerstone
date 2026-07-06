using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cornerstone.Common.Mediator;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCornerstoneMediator(params Assembly[] assemblies)
        {
            services.AddScoped<ISender, Sender>();
            RegisterOpenGeneric(services, assemblies, typeof(IRequestHandler<,>));
            RegisterOpenGeneric(services, assemblies, typeof(IPipelineBehavior<,>));
            return services;
        }
    }

    private static void RegisterOpenGeneric(IServiceCollection services, Assembly[] assemblies, Type openGenericInterface)
    {
        var registrations = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericInterface)
                .Select(i => (Interface: i, Implementation: t)));

        foreach (var (iface, impl) in registrations)
        {
    
            if (impl.IsGenericTypeDefinition)
                services.AddScoped(iface.GetGenericTypeDefinition(), impl);
            else
                services.AddScoped(iface, impl);
        }
    }
}
