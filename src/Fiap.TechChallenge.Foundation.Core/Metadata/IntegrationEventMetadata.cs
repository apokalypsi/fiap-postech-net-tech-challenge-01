using Fiap.TechChallenge.Foundation.Core.Messaging;

namespace Fiap.TechChallenge.Foundation.Core.Metadata;

/// <summary>
///     Metadado de um evento de integração.
/// </summary>
public sealed class IntegrationEventMetadata : BindingMetadata
{
    public IntegrationEventMetadata(Type handlerType, Type messageRequestType) : base(MessageType.IntegrationEvent,
        messageRequestType)
    {
        HandlerType = handlerType;
        HandlerMethod = HandlerType.GetMethod("Handle", new[] { messageRequestType });
    }
}