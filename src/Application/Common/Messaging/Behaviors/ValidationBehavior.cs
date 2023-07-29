using FluentValidation;
using MediatR;
using Serilog;

namespace Application.Common.Messaging.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> {
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        Log.Logger.Warning("ValidationBehavior constructor");
        _validators = validators;
    }


    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Log.Logger.Warning("ValidationBehavior Handle");
        if (_validators.Any())
        {
            Log.Logger.Warning("ValidationBehavior Validating");

            var context = new ValidationContext<TRequest>(request);
            var validationTasks = _validators.Select(v => v.ValidateAsync(context, cancellationToken));
            var validationResults = await Task.WhenAll(validationTasks);
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();
            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
        }
        return await next();
    }
}