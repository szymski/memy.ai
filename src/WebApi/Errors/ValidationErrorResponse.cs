using WebApi.Services;

namespace WebApi.Errors; 

public class ValidationErrorResponse : IErrorResponse {
    public class ErrorDetail {
        public required string FieldName { get; init; }
        public required string Message { get; init; }
        public required object AttemptedValue { get; init; }
    }
    
    public string ErrorCode => "validation_error";
    
    public string ErrorMessage { get; }

    public new IEnumerable<ErrorDetail> Details { get; init; }

    public ValidationErrorResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}