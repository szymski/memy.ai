using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Common.Messaging.Behaviors;

/// <summary>
/// DbBehavior injects <see cref="IAppDbContext"/> into <see cref="IDbRequestHandler"/> handlers.
/// </summary>
public class DbBehavior<TRequest, TResponse>(
    IAppDbContext context,
    IRequestHandler<TRequest, TResponse> handler) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> {

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (handler is IDbRequestHandler dbHandler)
        {
            dbHandler.Context = context;
        }

        return await next();
    }
}
