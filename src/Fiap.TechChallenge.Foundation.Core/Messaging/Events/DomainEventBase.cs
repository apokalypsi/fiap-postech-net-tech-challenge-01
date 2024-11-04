namespace Fiap.TechChallenge.Foundation.Core.Messaging.Events;

/// <summary>
///     Eventos de Domínio - são enviados dentro do contexto por Mediator
/// </summary>
public abstract class DomainEventBase : IDomainEvent
{
    protected DomainEventBase()
    {
        Id = Guid.NewGuid().ToString();
        Timestamp = DateTime.UtcNow;
    }

    public string Id { get; set; }
    public DateTime Timestamp { get; set; }
}