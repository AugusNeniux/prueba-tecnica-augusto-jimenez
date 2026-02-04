using CleanArchitecture.PracticalTest.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.PracticalTest.API.ErrorHandling;

public static class ProblemDetailsFactory
{
    public static ProblemDetails Validation(ValidationException ex, HttpContext context)
    {
        var problem = new ProblemDetails
        {
            Title = "Validation Failed",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Detail = "One or more validation errors occurred.",
            Instance = context.Request.Path
        };

        problem.Extensions["errors"] = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
        return problem;
    }

    public static ProblemDetails Domain(DomainException ex, HttpContext context)
    {
        var problem = new ProblemDetails
        {
            Title = "Business Rule Violation",
            Status = StatusCodes.Status422UnprocessableEntity,
            Type = "https://tools.ietf.org/html/rfc4918#section-11.2",
            Detail = ex.Message,
            Instance = context.Request.Path
        };

        problem.Extensions["errorCode"] = ex.ErrorCode;
        return problem;
    }

    public static ProblemDetails Unexpected(Exception ex, HttpContext context)
    {
        return new ProblemDetails
        {
            Title = "Internal Server Error",
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Detail = "An unexpected error occurred. Please try again later.",
            Instance = context.Request.Path
        };
    }
}
