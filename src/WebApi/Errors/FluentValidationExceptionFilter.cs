using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Errors;
using static WebApi.Errors.ErrorResponse;

namespace WebApi.Services;

public class FluentValidationExceptionFilter : IAsyncExceptionFilter, IExceptionFilter, IResultFilter {

    private readonly ILogger<FluentValidationExceptionFilter> _logger;
    private readonly IHostEnvironment _hostEnvironment;

    public FluentValidationExceptionFilter(
        ILogger<FluentValidationExceptionFilter> logger,
        IHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public void OnException(ExceptionContext context)
    {
        throw new NotImplementedException();
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.Exception is ValidationException e)
        {
            _logger.LogWarning("Fluent validation errors occured: {Details}", e.Errors);

            var validationException = new ValidationException("There were validation errors while processing your request.", e.Errors);
            context.Result = BuildValidationErrorResponse(validationException);
        }
        else if (context.Exception is {} ex)
        {
            _logger.LogError("Unhandled exception occured: {@Exception}", context.Exception);
            context.Result = new JsonResult(new UnknownError(ex.GetType().ToString(), ex.Message))
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Select(s => ((s.Key, s.Value.AttemptedValue, s.Value.Errors)))
                .SelectMany((pair) => pair.Errors.Select(e => new ValidationFailure(pair.Key, e.ErrorMessage)
                {
                    AttemptedValue = pair.AttemptedValue,
                }));
            var validationException = new ValidationException("Model state validation failed.", errors);

            _logger.LogWarning("Request model state invalid: {@Errors}", context.ModelState.Root.Children.Select(e => e.Errors));

            context.Result = BuildValidationErrorResponse(validationException);
            return;
        }

        if (context.Result is ObjectResult { StatusCode: var status } result)
        {
            var details = result.Value is {} v and not ProblemDetails ? v : default;
            ErrorResponse? newResult = status switch
            {
                StatusCodes.Status404NotFound => new NotFound(details),
                StatusCodes.Status500InternalServerError => new InternalServerError(details),
                StatusCodes.Status401Unauthorized => new Unauthorized(details),
                StatusCodes.Status403Forbidden => new Forbidden(details),
                StatusCodes.Status400BadRequest => new BadRequest(details),
                _ => null,
            };
            if (newResult is null)
                return;

            context.Result = new JsonResult(newResult)
            {
                StatusCode = status,
            };
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {

    }

    private static JsonResult BuildValidationErrorResponse(ValidationException e)
    {
        var response = new ValidationErrorResponse(e.Message)
        {
            Details = e.Errors.Select(err => new ValidationErrorResponse.ErrorDetail()
            {
                FieldName = err.PropertyName,
                Message = err.ErrorMessage,
                AttemptedValue = err.AttemptedValue,
            }),
        };

        return new(response)
        {
            StatusCode = StatusCodes.Status400BadRequest,
        };
    }
}