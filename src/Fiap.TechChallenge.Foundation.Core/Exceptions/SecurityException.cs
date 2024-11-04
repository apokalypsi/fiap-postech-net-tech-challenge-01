using Fiap.TechChallenge.Foundation.Core.Resilience;

namespace Fiap.TechChallenge.Foundation.Core.Exceptions;

/// <summary>
///     Erros que envolvem segurança.
/// </summary>
public sealed class SecurityException : ExceptionBaseWithResilienceType
{
    public SecurityException(string message) : base(message)
    {
        ResilienceErrorType = ResilienceErrorType.Permanent;
    }

    public SecurityException(string message, ResilienceErrorType resilienceErrorType) : base(message,
        resilienceErrorType)
    {
    }

    public SecurityException(string message, Exception innerException) : base(message, innerException)
    {
    }
}