namespace Clarity.Core.Models;

public class Result
{
    public bool IsSuccess { get; }
    public Dictionary<string, string[]> Errors { get; }

    protected Result(bool isSuccess, Dictionary<string, string[]> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }
    
    public static Dictionary<string, string[]> ToDict(string key, string[] value) =>
        new Dictionary<string, string[]>{{key, value}};
    
    public static Dictionary<string, string[]> ToDict(string key, string value) =>
        new Dictionary<string, string[]>{{key, [value]}};
    
    public static Result Success() => new(true, null!);
    public static Result Failure(Dictionary<string, string[]> errors) => new(false, errors);
}

public class Result<T> : Result
{
    public T Value { get; }

    private Result(T value, bool isSuccess, Dictionary<string, string[]> errors)
        : base(isSuccess, errors)
    {
        Value = value;
    }
        
    public static Result<T> Success(T value) => new(value, true, null!);
    public static Result<T> Failure(Dictionary<string, string[]> errors) => new(default!, false, errors);
}