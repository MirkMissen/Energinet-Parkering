using Case.Domain.ValueObjects;

namespace Case.Domain.Entities;

public sealed record User(UserId Id, Username Username, FullName FullName, DateTime CreatedAt);