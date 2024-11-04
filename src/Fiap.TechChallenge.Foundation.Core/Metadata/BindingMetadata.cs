using System.Reflection;
using Fiap.TechChallenge.Foundation.Core.Messaging;

namespace Fiap.TechChallenge.Foundation.Core.Metadata;

/// <summary>
///     Metadado de uma mensagem - abstração.
/// </summary>
public abstract class BindingMetadata
{
    protected BindingMetadata(MessageType bindingType, Type messageRequestType)
    {
        BindingType = bindingType;
        MessageRequestType = messageRequestType;
        MessageTypeName = messageRequestType.Name.ToLower();
    }

    public string MessageTypeName { get; }
    public MessageType BindingType { get; }
    public Type HandlerType { get; protected set; }
    public Type MessageRequestType { get; }
    public MethodInfo HandlerMethod { get; protected set; }
}