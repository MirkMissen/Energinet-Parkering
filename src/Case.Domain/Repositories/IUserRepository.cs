using Case.Domain.Entities;
using Case.Domain.ValueObjects;

namespace Case.Domain.Repositories;

public interface IUserRepository : IRepository
{
    Task<User> GetByIdAsync(UserId id);
}