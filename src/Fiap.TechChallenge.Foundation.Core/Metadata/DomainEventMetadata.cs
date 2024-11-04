using Fiap.TechChallenge.Foundation.Core.Messaging;

namespace Fiap.TechChallenge.Foundation.Core.Metadata;

/// <summary>
///     Metadado de um evento de domínio.
/// </summary>
public sealed class DomainEventMetadata : BindingMetadata
{
    public DomainEventMetadata(Type handlerType, Type messageRequestType) : base(MessageType.DomainEvent,
        messageRequestType)
    {
        HandlerType = handlerType;
        HandlerMethod = HandlerType.GetMethod("Handle", new[] { messageRequestType });
    }
}