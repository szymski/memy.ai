using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using MediatR;
using Serilog;

namespace Application.Common.Messaging.Behaviors;

public class DbBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> {

    private readonly IAppDbContext _context;
    // private readonly IRequestHandler<TRequest, TResponse> _handler;

    public DbBehavior(
        IAppDbContext context
        // IRequestHandler<TRequest, TResponse> handler
        )
    {
        _context = context;
        // _handler = handler;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Log.Logger.Warning("DbBehavior.Handle {0}", request);

        if (request is IDbRequestHandler handler)
        {
            Log.Logger.Warning("Handler is IDbRequestHandler, injecting Context");
            handler.Context = _context;
        }

        return await next();
    }
}