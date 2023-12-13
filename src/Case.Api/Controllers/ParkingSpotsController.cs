using Case.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Case.Api.Controllers;

[Route("parking-spots")]
public class ParkingSpotsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ParkingSpotsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetWeeklyParkingSpots.WeeklyParkingSpotDto>>> Get(
        [FromQuery] GetWeeklyParkingSpots.Query query)
    {
        var response = await _mediator.Send(query);
        return Ok(response);
    }
        
}