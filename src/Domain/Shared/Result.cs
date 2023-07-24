namespace Domain.Shared;

public class Result {
    public bool IsSuccess { get; init; }

    public static Result Success() => new() { IsSuccess = true };
    
    public static Result Failure() => new() { IsSuccess = false };
}

public class Result<T> {
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }

    private Result() {}

    public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
    
    public static Result<T> Failure() => new() { IsSuccess = false };
}