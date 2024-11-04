using Fiap.TechChallenge.Foundation.Core.Resilience;

namespace Fiap.TechChallenge.Foundation.Core.Exceptions;

/// <summary>
///     Excessão base com atributos de resiliência.
/// </summary>
public abstract class ExceptionBaseWithResilienceType : Exception
{
    protected ExceptionBaseWithResilienceType(string message)
        : base(message)
    {
    }

    protected ExceptionBaseWithResilienceType(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected ExceptionBaseWithResilienceType(string message, ResilienceErrorType resilienceErrorType)
        : this(message)
    {
        ResilienceErrorType = resilienceErrorType;
    }

    protected ExceptionBaseWithResilienceType(string message, ResilienceErrorType resilienceErrorType,
        Exception innerException)
        : this(message, innerException)
    {
        ResilienceErrorType = resilienceErrorType;
    }

    public ResilienceErrorType ResilienceErrorType { get; protected set; }
}