using Microsoft.Extensions.Logging;
using FluentValidation;
using MediatR;
using CleanArchitecture.PracticalTest.Application.Exceptions;
using CleanArchitecture.PracticalTest.Domain.Exceptions;
using CleanArchitecture.PracticalTest.Application.Contracts.ContextApplication;

namespace CleanArchitecture.PracticalTest.Application.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse>(ILogger<TRequest> logger, ILocalizer localizer) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ILocalizer _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            // Determinar el tipo de excepción para establecer el nivel de log
            switch (ex)
            {
                case DomainException _:
                case ValidationException _:
                case NotFoundException _:
                case ForbiddenException _:
                    // Logear excepción como warning
                    _logger.LogWarning("Warning: {Type}\n{Message}\nRequest: {Request}",
                        ex.GetType().Name,
                        Utility.ExceptionMessages(ex, _localizer),
                        request
                    );
                    break;
                default:
                    // Logear excepción como error
                    _logger.LogError("Unhandled Exception: {Message}\nRequest: {Request}\nTrace: {origin}",
                        Utility.ExceptionMessages(ex, _localizer),
                        request,
                        Utility.ParseStackTrace(ex)
                    );
                    break;
            }

            throw;
        }
    }


}
