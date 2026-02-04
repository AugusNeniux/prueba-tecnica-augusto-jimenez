using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.PracticalTest.API.ErrorHandling;

public interface IExceptionMapper
{
    bool CanHandle(Exception exception);
    ProblemDetails MapToProblemDetails(Exception exception, HttpContext context);
}
