using Case.Domain.Exceptions;

namespace Case.Application.Exceptions;

public class WeeklyParkingSpotReservationNotFound : CustomException {
    public Guid WeeklyParkingSpotId { get; }
    public Guid ReservationId { get; }
    
    public WeeklyParkingSpotReservationNotFound(Guid weeklyParkingSpotId, Guid reservationId) 
        : base($"Reservation with ID '{reservationId}' for parking spot ID '{weeklyParkingSpotId}' was not found.") {
        WeeklyParkingSpotId = weeklyParkingSpotId;
        ReservationId = reservationId;
    }
}