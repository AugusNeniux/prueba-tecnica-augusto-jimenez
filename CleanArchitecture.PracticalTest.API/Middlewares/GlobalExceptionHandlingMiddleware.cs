using CleanArchitecture.PracticalTest.API.ErrorHandling;
using Serilog;

namespace CleanArchitecture.PracticalTest.API.Middlewares;

public class GlobalExceptionHandlingMiddleware(RequestDelegate next, IEnumerable<IExceptionMapper> mappers)
{
    private readonly RequestDelegate _next = next;
    private readonly IEnumerable<IExceptionMapper> _mappers = mappers;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var mapper = _mappers.First(m => m.CanHandle(ex));
            var problem = mapper.MapToProblemDetails(ex, context);

            if (problem.Status >= 500)
                Log.Error(ex, "An unhandled exception occurred while processing the request.");

            context.Response.StatusCode = problem.Status ?? 500;
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}