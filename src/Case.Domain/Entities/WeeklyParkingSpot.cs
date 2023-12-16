using Case.Domain.Exceptions;
using Case.Domain.ValueObjects;

namespace Case.Domain.Entities;

public sealed class WeeklyParkingSpot
{
    public ParkingSpotId Id { get; }
    public Week Week { get; private set; }
    public ParkingSpotName Name { get; private set; }
    public IEnumerable<Reservation> Reservations => _reservations;

    private readonly HashSet<Reservation> _reservations = new();

    private WeeklyParkingSpot(ParkingSpotId id, Week week, ParkingSpotName name)
    {
        Id = id;
        Week = week;
        Name = name;
    }

    public static WeeklyParkingSpot Create(ParkingSpotId id, Week week, ParkingSpotName name) => new(id, week, name);

    public void AddReservation(Reservation reservation, Date now) {
        ValidateReservation(reservation, now);

        if (_reservations.Any(x => x.Date == reservation.Date))
        {
            throw new ParkingSpotAlreadyReservedException(Name, reservation.Date.Value.Date);
        }

        _reservations.Add(reservation);
    }

    public void UpdateReservation(Reservation reservation, Date now) {
        ValidateReservation(reservation, now);
        
        this._reservations.RemoveWhere(x => x.Id.Equals(reservation.Id));
        this._reservations.Add(reservation);
    }

    private void ValidateReservation(Reservation reservation, Date now) {
        var isInvalidDate = reservation.Date < Week.From ||
                            reservation.Date > Week.To ||
                            reservation.Date < now;
        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservation.Date.Value.Date);
        }
        
    }
}