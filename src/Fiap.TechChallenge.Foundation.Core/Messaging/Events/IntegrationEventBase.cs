namespace Fiap.TechChallenge.Foundation.Core.Messaging.Events;

/// <inheritdoc />
/// <summary>
///     Eventos de Integração - passarão por algum mensageiro - BUS
/// </summary>
public abstract class IntegrationEventBase : IIntegrationEvent
{
    protected IntegrationEventBase()
    {
        Id = Guid.NewGuid().ToString();
        Timestamp = DateTime.UtcNow;
    }

    public string Id { get; set; }
    public DateTime Timestamp { get; set; }
    public abstract string EndpointName { get; }
}