namespace Fiap.TechChallenge.Foundation.Core.Messaging.Events;

/// <summary>
///     Handler para eventos de integraçao que o (micro)serviço consome.
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IIntegrationEventHandler<in TEvent> where TEvent : IntegrationEventBase
{
    Task Handle(TEvent message);
}