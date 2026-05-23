namespace CleanArchitecture.Domain.Abstractions;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvent = new();

    protected Entity(Guid id)
    {
        Id = id;
    }
    public Guid Id {get; init;}

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvent.ToList();
    }

    public void ClearDomainEvents()
    {
        _domainEvent.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvent.Add(domainEvent);
    }
}