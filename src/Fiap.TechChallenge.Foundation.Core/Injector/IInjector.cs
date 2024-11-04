using System.Reflection;
using Fiap.TechChallenge.Foundation.Core.Messaging;
using Fiap.TechChallenge.Foundation.Core.Metadata;

namespace Fiap.TechChallenge.Foundation.Core.Injector;

/// <summary>
///     Abstração para a gestão de injeção de dependência.
/// </summary>
public interface IInjector : IDisposable
{
    IServiceResolver Resolver { get; }

    IInjectorScope BeginScope(InjectorScopingType scoping);

    void Verify();

    IInjector AddTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;

    IInjector AddTransient(Type service, Type implementation);
    IInjector AddTransient(Type openGenericServiceType, params Assembly[] assemblies);
    IInjector AddTransientCollection(Type openGenericServiceType, params Assembly[] assemblies);
    IInjector AddTransientCollection<TService>(params Assembly[] assemblies) where TService : class;

    IInjector AddTransientFactory<TService>(Func<TService> factory)
        where TService : class;

    IInjector AddDecoratorTransient(Type service, Type decorator);

    IInjector AddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;

    IInjector AddSingleton(Type service, Type implementation);
    IInjector AddSingleton(Type openGenericServiceType, params Assembly[] assemblies);

    IInjector AddSingletonFactory<TService>(Func<TService> factory)
        where TService : class;

    IInjector AddScoped<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;

    IInjector AddScoped(Type service, Type implementation);

    IInjector AddScopedFactory<TService>(Func<TService> factory)
        where TService : class;

    IInjector AddInstance<TService>(TService service)
        where TService : class;

    /// <summary>
    ///     Obtem lista de metadata de acordo com o tipo.
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="openGenericType"></param>
    /// <returns></returns>
    IEnumerable<BindingMetadata> GetRegistrationsOfType(MessageType messageType, Type openGenericType);
}