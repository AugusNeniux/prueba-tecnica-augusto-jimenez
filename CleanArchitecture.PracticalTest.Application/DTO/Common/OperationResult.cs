namespace CleanArchitecture.PracticalTest.Application.DTO.Common;

public class OperationResult<T>
{
    public T Data { get; set; } = default!;
    public List<string>? Warnings { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}

public static class OperationResult
{
    public static OperationResult<T> With<T>(
        T data,
        List<string>? warnings = null,
        Dictionary<string, object>? metadata = null) =>
        new() { Data = data, Warnings = warnings, Metadata = metadata };
}
