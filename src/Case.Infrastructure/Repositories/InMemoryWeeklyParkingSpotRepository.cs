using Case.Domain;
using Case.Domain.Abstractions;
using Case.Domain.Entities;
using Case.Domain.Repositories;
using Case.Domain.ValueObjects;

namespace Case.Infrastructure.Repositories;

internal sealed class InMemoryWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository, IUnitOfWork
{
    private static List<WeeklyParkingSpot> WeeklyParkingSpots;
    
    public InMemoryWeeklyParkingSpotRepository(IClock clock)
    {
        if (WeeklyParkingSpots == null)
        {
            WeeklyParkingSpots = new List<WeeklyParkingSpot>
            {
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(clock.Current()),
                    "P1"),
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(clock.Current()),
                    "P2"),
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(clock.Current()),
                    "P3"),
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(clock.Current()),
                    "P4"),
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(clock.Current()),
                    "P5"),
            };
        }
    }

    public async Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync()
    {
        await Task.CompletedTask;
        return WeeklyParkingSpots;
    }

    public async Task<IEnumerable<WeeklyParkingSpot>> GetByWeekAsync(Week week)
    {
        await Task.CompletedTask;
        return WeeklyParkingSpots
            .Where(x => x.Week == week);
    }

    public async Task<WeeklyParkingSpot> GetAsync(ParkingSpotId id)
    {
        await Task.CompletedTask;
        return WeeklyParkingSpots.SingleOrDefault(x => x.Id == id);
    }

    public Task AddAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        WeeklyParkingSpots.Add(weeklyParkingSpot);
        return Task.CompletedTask;
    }

    public IUnitOfWork UnitOfWork => this;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(1);
    }
}