using Fiap.TechChallenge.Foundation.Core.Messaging.Events;

namespace Fiap.TechChallenge.Foundation.Core.Domain;

/// <summary>
///     Representa uma entidade persistível na aplicação.
/// </summary>
public interface IEntity
{
    IReadOnlyCollection<DomainEventBase> GetEvents();
    void ClearEvents();
}

/// <inheritdoc />
/// <summary>
///     Representa uma entidade persistível com Id na aplicação.
/// </summary>
/// <typeparam name="TIdType"></typeparam>
public interface IEntity<TIdType> : IEntity
{
    TIdType Id { get; }
    void SetId(TIdType id);
}