using Fiap.TechChallenge.Foundation.Core.Injector;

namespace Fiap.TechChallenge.Foundation.Core.Metadata;

public static class ApplicationMetadataExtensions
{
    public static IInjector UseApplicationMetadata(this IInjector injector)
    {
        return injector.AddSingleton<IBindingMetadataService, BindingMetadataService>();
    }
}