using Case.Domain.Entities;
using Case.Domain.ValueObjects;

namespace Case.Domain.Repositories;

public interface IWeeklyParkingSpotRepository : IRepository
{
    Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync();
    Task<IEnumerable<WeeklyParkingSpot>> GetByWeekAsync(Week week);
    Task<WeeklyParkingSpot> GetAsync(ParkingSpotId id);
    Task AddAsync(WeeklyParkingSpot weeklyParkingSpot);
}