namespace Fiap.TechChallenge.Foundation.Core.Metadata;

public static class BindingMetadataExtensions
{
    public static IntegrationEventMetadata AsIntegrationEventMetadata(this BindingMetadata bindingMetadata)
    {
        return (IntegrationEventMetadata)bindingMetadata;
    }

    public static CommandMetadata AsCommandMetadata(this BindingMetadata bindingMetadata)
    {
        return (CommandMetadata)bindingMetadata;
    }
}