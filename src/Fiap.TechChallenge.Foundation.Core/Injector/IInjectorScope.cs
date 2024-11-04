namespace Fiap.TechChallenge.Foundation.Core.Injector;

/// <summary>
///     Representa um escopo de execução para o injetor de dependências.
/// </summary>
public interface IInjectorScope : IServiceResolver, IDisposable
{
}