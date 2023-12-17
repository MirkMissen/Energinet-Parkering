using Case.Application.Exceptions;
using Case.Domain.Abstractions;
using Case.Domain.Repositories;
using Case.Domain.ValueObjects;
using MediatR;

namespace Case.Application; 

public static class ChangeReservationLicencePlate {

    public sealed record Command(Guid ReservationId, Guid ParkingSpotId, string Numberplate) : IRequest {
        public Command() : this(Guid.Empty, Guid.Empty, string.Empty) { }
        public Command(Guid ReservationId, string Numberplate) : this(ReservationId, Guid.Empty, Numberplate) { }
    }
    
    internal class Handler : IRequestHandler<Command> {
        
        private readonly IWeeklyParkingSpotRepository _repository;
        private readonly IClock _clock;

        public Handler(IWeeklyParkingSpotRepository repository, IClock clock) {
            _repository = repository;
            _clock = clock;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) {
            var weeklyParkingSpot = await _repository.GetAsync(request.ParkingSpotId);
            var reservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id.Value.Equals(request.ReservationId));

            if (reservation is null) {
                throw new WeeklyParkingSpotReservationNotFound(request.ParkingSpotId, request.ReservationId);
            }
            
            var updatedReservation = reservation with {
                LicensePlate = new LicensePlate(request.Numberplate)
            };
            
            weeklyParkingSpot.UpdateReservation(updatedReservation, new Date(_clock.Current()));
            
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            return new Unit();
        }
        
    }

}