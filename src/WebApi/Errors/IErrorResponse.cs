namespace WebApi.Errors;

public interface IErrorResponse {
    string ErrorCode { get; }
    string ErrorMessage { get; }
}