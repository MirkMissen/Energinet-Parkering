using Case.Application.Exceptions;
using Case.Domain.Abstractions;
using Case.Domain.Entities;
using Case.Domain.Repositories;
using Case.Domain.ValueObjects;
using MediatR;

namespace Case.Application;

public static class ReserveParkingSpot
{
    public sealed record Command(Guid ParkingSpotId, Guid ReservationId, Guid UserId,
        string LicensePlate, DateTime Date) : IRequest;

    internal class Handler : IRequestHandler<Command>
    {
        private readonly IWeeklyParkingSpotRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IClock _clock;

        public Handler(IWeeklyParkingSpotRepository repository, IUserRepository userRepository, IClock clock)
        {
            _repository = repository;
            _userRepository = userRepository;
            _clock = clock;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var week = new Week(_clock.Current());
            var parkingSpotId = new ParkingSpotId(request.ParkingSpotId);
            var weeklyParkingSpots = (await _repository.GetByWeekAsync(week)).ToList();
            var parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(x => x.Id == parkingSpotId);

            if (parkingSpotToReserve is null)
            {
                throw new WeeklyParkingSpotNotFoundException(request.ParkingSpotId);
            }

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            var reservation = new Reservation(request.ReservationId, user.Id, new EmployeeName(user.FullName),
            request.LicensePlate, new Date(request.Date));

            parkingSpotToReserve.AddReservation(reservation, new Date(_clock.Current()));

            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return new Unit();
        }
    }
}