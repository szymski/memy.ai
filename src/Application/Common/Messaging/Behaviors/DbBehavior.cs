using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Common.Messaging.Behaviors;

/// <summary>
/// DbBehavior injects <see cref="IAppDbContext"/> into <see cref="IDbRequestHandler"/> handlers.
/// </summary>
public class DbBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> {

    private readonly IAppDbContext _context;
    private readonly IRequestHandler<TRequest, TResponse> _handler;

    public DbBehavior(
        IAppDbContext context,
        IRequestHandler<TRequest, TResponse> handler
    )
    {
        _context = context;
        _handler = handler;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_handler is IDbRequestHandler handler)
        {
            handler.Context = _context;
        }

        return await next();
    }
}