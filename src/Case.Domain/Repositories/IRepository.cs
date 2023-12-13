namespace Case.Domain;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}
