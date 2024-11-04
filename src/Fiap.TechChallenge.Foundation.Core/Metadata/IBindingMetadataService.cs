using Fiap.TechChallenge.Foundation.Core.Injector;
using Fiap.TechChallenge.Foundation.Core.Messaging;

namespace Fiap.TechChallenge.Foundation.Core.Metadata;

public interface IBindingMetadataService
{
    BindingMetadata GetBindingFromMessage(IMessage message);
    BindingMetadata GetBindingFromMessage(string messageName);
    bool HasBindingFromMessage(string messageName);
    bool HasBindingFromMessage(IMessage message);
    void LoadFromInjector(IInjector injector);
    IEnumerable<BindingMetadata> GetBindingsFromType(MessageType bindingType);
}