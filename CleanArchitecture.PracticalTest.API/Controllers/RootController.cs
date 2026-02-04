namespace CleanArchitecture.PracticalTest.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
[Route("/api/v{version:apiVersion}/[controller]")]
public class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;
    protected readonly ILocalizer _localizer;

    protected BaseController(IMediator mediator, ILocalizer localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }
}