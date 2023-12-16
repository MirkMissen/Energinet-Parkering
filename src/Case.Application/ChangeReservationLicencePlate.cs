using Case.Domain.Abstractions;
using Case.Domain.Repositories;
using Case.Domain.ValueObjects;
using MediatR;

namespace Case.Application; 

public static class ChangeReservationLicencePlate {
    
    public sealed record Command(Guid ReservationId, string Numberplate) : IRequest;
    
    internal class Handler : IRequestHandler<Command> {
        
        private readonly IWeeklyParkingSpotRepository _repository;
        private readonly IClock _clock;

        public Handler(IWeeklyParkingSpotRepository repository, IClock clock) {
            _repository = repository;
            _clock = clock;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) {

            var all = await this._repository.GetAllAsync();

            var weeklyParkingSpot = all.Single(x => x.Reservations.Any(reservation => reservation.Id.Value.Equals(request.ReservationId)));
            var reservation = weeklyParkingSpot.Reservations.Single(x => x.Id.Value.Equals(request.ReservationId));
            
            var updatedReservation = reservation with {
                LicensePlate = new LicensePlate(request.Numberplate)
            };
            
            weeklyParkingSpot.UpdateReservation(updatedReservation, new Date(_clock.Current()));
            
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            return new Unit();
        }
        
    }

}