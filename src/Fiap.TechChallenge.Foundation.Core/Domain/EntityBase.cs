using Fiap.TechChallenge.Foundation.Core.Messaging.Events;
using Fiap.TechChallenge.Foundation.Core.Validations;

namespace Fiap.TechChallenge.Foundation.Core.Domain;

/// <inheritdoc />
/// <summary>
///     Abstração básica de uma entidade.
/// </summary>
/// <typeparam name="TIdType"></typeparam>
public abstract class EntityBase<TIdType> : IEntity<TIdType>
{
    private List<DomainEventBase> _domainEvents;

    public virtual TIdType Id { get; protected set; }

    public virtual void SetId(TIdType id)
    {
        Contract.Requires.IsNotNull(id, nameof(id)).Guard();
        Id = id;
    }

    public virtual IReadOnlyCollection<DomainEventBase> GetEvents()
    {
        return _domainEvents ?? (IReadOnlyCollection<DomainEventBase>)new List<DomainEventBase>();
    }

    public virtual void ClearEvents()
    {
        _domainEvents?.Clear();
    }

    public override bool Equals(object obj)
    {
        return obj is EntityBase<TIdType> other && Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    protected virtual void Publish(DomainEventBase domainEvent)
    {
        _domainEvents ??= new List<DomainEventBase>();
        _domainEvents.Add(domainEvent);
    }
}