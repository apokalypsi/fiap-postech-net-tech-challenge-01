namespace Fiap.TechChallenge.Foundation.Core.Injector;

/// <summary>
///     Abstração de serviço de resolução de inversão de controle.
/// </summary>
/// <returns></returns>
public interface IServiceResolver
{
    /// <summary>
    ///     Resolve um serviço genérico.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    TService Resolve<TService>()
        where TService : class;

    /// <summary>
    ///     Resolve um serviço pelo tipo.
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    object Resolve(Type serviceType);

    IEnumerable<TService> ResolveAll<TService>()
        where TService : class;

    IEnumerable<object> ResolveAll(Type serviceType);

    TService TryResolve<TService>()
        where TService : class;

    object TryResolve(Type serviceType);
}