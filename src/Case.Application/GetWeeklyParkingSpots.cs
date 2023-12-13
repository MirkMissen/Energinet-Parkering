using Case.Domain.Entities;
using Case.Domain.Repositories;
using Case.Domain.ValueObjects;
using MediatR;

namespace Case.Application;

public static class GetWeeklyParkingSpots
{
    public sealed record Query(DateTime? Date) : IRequest<IEnumerable<WeeklyParkingSpotDto>>;

    internal class Handler : IRequestHandler<Query, IEnumerable<WeeklyParkingSpotDto>>
    {
        private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;

        public Handler(IWeeklyParkingSpotRepository weeklyParkingSpotRepository)
        {
            _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
        }

        public async Task<IEnumerable<WeeklyParkingSpotDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var week = request.Date.HasValue ? new Week(request.Date.Value) : null;

            var weeklyParkingSpots = (await _weeklyParkingSpotRepository.GetByWeekAsync(week)).ToList();

            return weeklyParkingSpots.Select(x => x.AsDto());
        }
    }

    private static WeeklyParkingSpotDto AsDto(this WeeklyParkingSpot entity)
        => new()
        {
            Id = entity.Id.Value.ToString(),
            Name = entity.Name,
            From = entity.Week.From.Value.DateTime,
            To = entity.Week.To.Value.DateTime,
            Reservations = entity.Reservations.Select(x => new ReservationDto
            {
                Id = x.Id,
                EmployeeName = x.EmployeeName,
                UserId = x.UserId.Value,
                Date = x.Date.Value.Date,
                LicensePlate = x.LicensePlate.Value
            })
        };

    public class WeeklyParkingSpotDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<ReservationDto> Reservations { get; set; }
    }

    public class ReservationDto
    {
        public Guid Id { get; set; }
        public string EmployeeName { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public string LicensePlate { get; set; }
    }
}