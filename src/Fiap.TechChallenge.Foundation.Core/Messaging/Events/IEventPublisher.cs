namespace Fiap.TechChallenge.Foundation.Core.Messaging.Events;

/// <summary>
///     Abstração para publicação de eventos de domínio - mediador.
/// </summary>
public interface IEventPublisher
{
    Task Publish<TEvent>(TEvent @event) where TEvent : DomainEventBase;
}