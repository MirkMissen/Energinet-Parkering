using Case.Domain;
using Case.Domain.Entities;
using Case.Domain.Repositories;
using Case.Domain.ValueObjects;

namespace Case.Infrastructure.Repositories;

internal class InMemoryUserRepository : IUserRepository, IUnitOfWork
{
    private readonly List<User> _users = new()
    {
        new User(new UserId(Guid.Parse("00000000-0000-0000-0000-000000000001")), new Username("User1"), new FullName("User Number 1"), DateTime.Now),
        new User(new UserId(Guid.Parse("00000000-0000-0000-0000-000000000002")), new Username("User2"), new FullName("User Number 2"), DateTime.Now),
        new User(new UserId(Guid.Parse("00000000-0000-0000-0000-000000000003")), new Username("User3"), new FullName("User Number 3"), DateTime.Now),
        new User(new UserId(Guid.Parse("00000000-0000-0000-0000-000000000004")), new Username("User4"), new FullName("User Number 4"), DateTime.Now),
        new User(new UserId(Guid.Parse("00000000-0000-0000-0000-000000000005")), new Username("User5"), new FullName("User Number 5"), DateTime.Now),
    };

    public IUnitOfWork UnitOfWork => this;
    public async Task<User> GetByIdAsync(UserId id)
    {
        await Task.CompletedTask;
        return _users.FirstOrDefault(user => user.Id == id);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(1);
    }
}