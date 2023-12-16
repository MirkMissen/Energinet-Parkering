using Case.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Case.Api.Controllers;

[Route("parking-spots")]
public class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{parkingSpotId:guid}/reservations")]
    public async Task<ActionResult> Post(Guid parkingSpotId, [FromBody]ReserveParkingSpot.Command command)
    {
        await _mediator.Send(command with
        {
            ReservationId = Guid.NewGuid(),
            ParkingSpotId = parkingSpotId
        });
        return NoContent();
    }
    
    [HttpPut("{parkingSpotId:guid}/reservations")]
    public async Task<ActionResult> Post(Guid parkingSpotId, [FromBody]ChangeReservationLicencePlate.Command command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    
}