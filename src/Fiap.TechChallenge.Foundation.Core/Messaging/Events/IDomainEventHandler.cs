namespace Fiap.TechChallenge.Foundation.Core.Messaging.Events;

/// <summary>
///     Abstração para assinatura de evento de domínio - interno do Contexto delimitado.
/// </summary>
public interface IDomainEventHandler<in TEvent> where TEvent : DomainEventBase
{
    Task Handle(TEvent message);
}