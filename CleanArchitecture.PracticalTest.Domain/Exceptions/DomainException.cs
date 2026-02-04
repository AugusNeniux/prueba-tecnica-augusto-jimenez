namespace CleanArchitecture.PracticalTest.Domain.Exceptions;

public class DomainException(string messageKey, params object[]? args) : Exception(messageKey)
{
    public object[]? Args { get; } = args;
    public string ErrorCode { get; } = messageKey;
}
