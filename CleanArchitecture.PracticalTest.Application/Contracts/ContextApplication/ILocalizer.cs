namespace CleanArchitecture.PracticalTest.Application.Contracts.ContextApplication;

public interface ILocalizer
{
    string GetValidationMessage(string key, params object[] args);
    string GetLoggerMessage(string key, params object[] args);
    string GetExceptionMessage(string key, params object[] args);
    string GetResponseMessage(string key, params object[] args);
    string GetDomainConcept(string key, params object[] args);
    string GetEnumValue(string key, params object[] args);
}