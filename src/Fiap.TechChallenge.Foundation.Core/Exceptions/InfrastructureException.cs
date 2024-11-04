using Fiap.TechChallenge.Foundation.Core.Resilience;

namespace Fiap.TechChallenge.Foundation.Core.Exceptions;

/// <inheritdoc />
/// <summary>
///     Excessões de infraestrutura (geralmente) indicam um erro que pode ser reestabelecido.
///     Políticas de retry podem ser aplicadas.
/// </summary>
public sealed class InfrastructureException : ExceptionBaseWithResilienceType
{
    public InfrastructureException(string message) : base(message)
    {
        ResilienceErrorType = ResilienceErrorType.Transient;
    }

    public InfrastructureException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public InfrastructureException(string message, ResilienceErrorType resilienceErrorType)
        : base(message, resilienceErrorType)
    {
    }

    public InfrastructureException(string message, ResilienceErrorType resilienceErrorType,
        Exception innerException)
        : base(message, resilienceErrorType, innerException)
    {
    }
}