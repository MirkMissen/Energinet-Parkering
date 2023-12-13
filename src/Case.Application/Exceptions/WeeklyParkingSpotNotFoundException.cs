using Case.Domain.Exceptions;

namespace Case.Application.Exceptions;

public sealed class WeeklyParkingSpotNotFoundException : CustomException
{
    public Guid? Id { get; }

    public WeeklyParkingSpotNotFoundException(Guid id) 
        : base($"Weekly parking spot with ID: {id} was not found.")
    {
        Id = id;
    }
}