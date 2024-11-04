namespace Fiap.TechChallenge.Foundation.Core.Messaging.Events;

/// <summary>
///     Evento de integração.
/// </summary>
public interface IIntegrationEvent : IEvent
{
    string Id { get; set; }
    DateTime Timestamp { get; set; }
    string EndpointName { get; }
}