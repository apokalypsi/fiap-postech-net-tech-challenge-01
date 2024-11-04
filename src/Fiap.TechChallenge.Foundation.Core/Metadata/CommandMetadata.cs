using Fiap.TechChallenge.Foundation.Core.Messaging;

namespace Fiap.TechChallenge.Foundation.Core.Metadata;

/// <summary>
///     Metadado de um comando.
/// </summary>
public sealed class CommandMetadata : BindingMetadata
{
    public CommandMetadata(Type handlerType, Type messageRequestType, Type messageResponseType)
        : base(MessageType.Command, messageRequestType)
    {
        MessageResponseType = messageResponseType;
        HandlerType = handlerType;
        HandlerMethod = HandlerType.GetMethod("Handle", new[] { messageRequestType });
    }

    public Type MessageResponseType { get; }
}