using Fiap.TechChallenge.Foundation.Core.Messaging;

namespace Fiap.TechChallenge.Foundation.Core.Metadata;

/// <summary>
///     Metadado de uma query.
/// </summary>
public sealed class QueryMetadata : BindingMetadata
{
    public QueryMetadata(Type handlerType, Type messageRequestType, Type messageResponseType)
        : base(MessageType.Query, messageRequestType)
    {
        MessageResponseType = messageResponseType;
        HandlerType = handlerType;
        HandlerMethod = HandlerType.GetMethod("Handle", new[] { messageRequestType });
    }

    public Type MessageResponseType { get; }
}