using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApi.Errors;

public abstract record ErrorResponse(string ErrorCode, string ErrorMessage) {
    public sealed record NotFound(object Details) : ErrorResponse("not_found", "The requested resource was not found.") {}

    public sealed record Unauthorized(object Details) : ErrorResponse("unauthorized", "Please sign in.") {}

    public sealed record Forbidden(object Details) : ErrorResponse("forbidden", "Try again later.") {}

    public sealed record BadRequest(object Details) : ErrorResponse("bad_request", "Please correct your request and try again.") {}

    public sealed record InternalServerError(object Details) : ErrorResponse("internal", "Please try again later.") {}

    public sealed record UnknownError(UnknownError.UnknownErrorDetails Details) : ErrorResponse("unknown", "Please try again.") {
        public UnknownError(string ErrorCode, string ErrorMessage) : this(new UnknownErrorDetails(ErrorCode, ErrorMessage))
        {
        }
        
        public record UnknownErrorDetails(string ExceptionType, string Message);
    }
}