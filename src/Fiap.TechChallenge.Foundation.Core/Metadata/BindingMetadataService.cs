using Fiap.TechChallenge.Foundation.Core.Injector;
using Fiap.TechChallenge.Foundation.Core.Messaging;
using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;
using Fiap.TechChallenge.Foundation.Core.Messaging.Events;
using Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

namespace Fiap.TechChallenge.Foundation.Core.Metadata;

public sealed class BindingMetadataService : IBindingMetadataService
{
    private readonly Dictionary<string, BindingMetadata> _bindingMetadata;
    private readonly Dictionary<MessageType, List<BindingMetadata>> _bindingMetadataPerMessageType;

    public BindingMetadataService()
    {
        _bindingMetadata = new Dictionary<string, BindingMetadata>();
        _bindingMetadataPerMessageType = new Dictionary<MessageType, List<BindingMetadata>>();
    }

    public bool HasBindingFromMessage(IMessage message)
    {
        return HasBindingFromMessage(message.GetType().Name.ToLower());
    }

    public void LoadFromInjector(IInjector injector)
    {
        var bindingList = new List<BindingMetadata>();
        bindingList.AddRange(injector.GetRegistrationsOfType(MessageType.Command, typeof(ICommandHandler<,>)));
        bindingList.AddRange(
            injector.GetRegistrationsOfType(MessageType.DomainEvent, typeof(IDomainEventHandler<>)));
        bindingList.AddRange(injector.GetRegistrationsOfType(MessageType.IntegrationEvent,
            typeof(IIntegrationEventHandler<>)));
        bindingList.AddRange(injector.GetRegistrationsOfType(MessageType.Query, typeof(IQueryHandler<,>)));
        LoadBinbingsByName(bindingList);
        LoadBindingsByType(bindingList);
    }

    public bool HasBindingFromMessage(string messageName)
    {
        return _bindingMetadata.ContainsKey(messageName?.ToLower());
    }

    public IEnumerable<BindingMetadata> GetBindingsFromType(MessageType bindingType)
    {
        return _bindingMetadataPerMessageType.ContainsKey(bindingType)
            ? _bindingMetadataPerMessageType[bindingType]
            : new List<BindingMetadata>();
    }

    public BindingMetadata GetBindingFromMessage(IMessage message)
    {
        return GetBindingFromMessage(message.GetType().Name.ToLower());
    }

    public BindingMetadata GetBindingFromMessage(string messageName)
    {
        return _bindingMetadata[messageName?.ToLower()];
    }

    private void LoadBindingsByType(IEnumerable<BindingMetadata> bindingsFromScan)
    {
        var groupedByType =
            (from b in bindingsFromScan
                group b by new { b.BindingType }
                into g
                select new { g.Key.BindingType, Bindings = g.ToList() })
            .ToList();

        foreach (var binding in groupedByType)
            if (_bindingMetadataPerMessageType.ContainsKey(binding.BindingType))
                _bindingMetadataPerMessageType[binding.BindingType].AddRange(binding.Bindings);
            else
                _bindingMetadataPerMessageType.Add(binding.BindingType, binding.Bindings);
    }

    private void LoadBinbingsByName(IEnumerable<BindingMetadata> bindingsFromScan)
    {
        foreach (var binding in bindingsFromScan)
            _bindingMetadata.Add(binding.MessageTypeName, binding);
    }
}