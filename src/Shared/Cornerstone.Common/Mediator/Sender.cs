using Microsoft.Extensions.DependencyInjection;

namespace Cornerstone.Common.Mediator;

public sealed class Sender(IServiceProvider serviceProvider) : ISender
{
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = serviceProvider.GetRequiredService(handlerType);
        var handleMethod = handlerType.GetMethod("Handle")!;

        RequestHandlerDelegate<TResponse> pipeline = () =>
            (Task<TResponse>)handleMethod.Invoke(handler, [request, cancellationToken])!;

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = serviceProvider.GetServices(behaviorType).Reverse();
        var behaviorHandleMethod = behaviorType.GetMethod("Handle")!;

        foreach (var behavior in behaviors)
        {
            var next = pipeline;
            pipeline = () => (Task<TResponse>)behaviorHandleMethod.Invoke(behavior, [request, next, cancellationToken])!;
        }

        return await pipeline();
    }
}
