namespace CleanArchitecture.PracticalTest.Application.DTO.Common;

public class APIResponse<T>
{
    public string? Message { get; set; }
    public OperationResult<T> Result { get; set; } = default!;
}

public static class APIResponse
{
    public static APIResponse<T> From<T>(OperationResult<T> result, string? message = "Success") =>
        new() { Result = result, Message = message };

}