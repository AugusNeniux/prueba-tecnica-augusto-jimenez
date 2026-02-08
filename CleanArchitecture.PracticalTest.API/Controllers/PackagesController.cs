using CleanArchitecture.PracticalTest.API.Controllers.Requests;
using CleanArchitecture.PracticalTest.Application.DTO.Common;
using CleanArchitecture.PracticalTest.Application.Packages.AssignRoute;
using CleanArchitecture.PracticalTest.Application.Packages.Create;
using CleanArchitecture.PracticalTest.Application.Packages.GetById;
using CleanArchitecture.PracticalTest.Application.Packages.UpdateStatus;

namespace CleanArchitecture.PracticalTest.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
[Route("/api/v{version:apiVersion}/packages")]
public class PackagesController : BaseController
{
    public PackagesController(IMediator mediator, ILocalizer localizer): base(mediator, localizer)
    {

    }

    [HttpPost]
    public async Task<ActionResult<APIResponse<Guid>>> Create([FromBody] CreatePackageCommand command)
    {
        var result = await _mediator.Send(command);

        var message = _localizer.GetResponseMessage("Package.Created");

        return Ok(APIResponse.From(result, message));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<APIResponse<PackageDetailsDto>>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetPackageByIdQuery(id));
        var message = _localizer.GetResponseMessage("Package.Retrieved");
        return Ok(APIResponse.From(result, message));
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<APIResponse<Guid>>> UpdateStatus(Guid id, [FromBody] UpdatePackageStatusRequest body)
    {
        var command = new UpdatePackageStatusCommand
        {
            PackageId = id,
            NewStatus = body.NewStatus,
            Reason = body.Reason
        };

        var result = await _mediator.Send(command);
        var message = _localizer.GetResponseMessage("Package.StatusUpdated");

        return Ok(APIResponse.From(result, message));
    }

    [HttpPost("{id:guid}/assign-route")]
    public async Task<ActionResult<APIResponse<AssignRouteResultDto>>> AssignRoute(Guid id, [FromBody] AssignRouteRequest body)
    {
        var command = new AssignRouteCommand
        {
            PackageId = id,
            Origin = body.Origin,
            Destination = body.Destination,
            DistanceKm = body.DistanceKm,
            EstimatedTimeHours = body.EstimatedTimeHours
        };

        var result = await _mediator.Send(command);
        var message = _localizer.GetResponseMessage("Package.RouteAssigned");

        return Ok(APIResponse.From(result, message));
    }
}
