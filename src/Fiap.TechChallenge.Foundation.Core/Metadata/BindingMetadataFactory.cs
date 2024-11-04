using Fiap.TechChallenge.Foundation.Core.Messaging;

namespace Fiap.TechChallenge.Foundation.Core.Metadata;

public static class BindingMetadataFactory
{
    public static BindingMetadata Create(MessageType bindingType, Type handlerType, Type[] requestArguments)
    {
        switch (bindingType)
        {
            case MessageType.Command:
                return new CommandMetadata(handlerType, requestArguments.FirstOrDefault(),
                    requestArguments.LastOrDefault());
            case MessageType.DomainEvent:
                return new DomainEventMetadata(handlerType, requestArguments.FirstOrDefault());
            case MessageType.IntegrationEvent:
                return new IntegrationEventMetadata(handlerType, requestArguments.FirstOrDefault());
            case MessageType.Query:
                return new QueryMetadata(handlerType, requestArguments.FirstOrDefault(),
                    requestArguments.LastOrDefault());
        }

        throw new InvalidOperationException($"BindigType {bindingType} does not exists.");
    }
}