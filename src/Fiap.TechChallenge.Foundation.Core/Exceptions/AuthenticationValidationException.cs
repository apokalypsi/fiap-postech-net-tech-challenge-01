using Fiap.TechChallenge.Foundation.Core.Resilience;
using Fiap.TechChallenge.Foundation.Core.Validations.Results;

namespace Fiap.TechChallenge.Foundation.Core.Exceptions;

/// <summary>
///     Erros de autenticação.
/// </summary>
public sealed class AuthenticationValidationException : ExceptionBaseWithResilienceType
{
    public AuthenticationValidationException(string message) : base(message)
    {
        ResilienceErrorType = ResilienceErrorType.Permanent;
    }

    public AuthenticationValidationException(string code, string message)
        : base(message)
    {
        Code = code;
        ResilienceErrorType = ResilienceErrorType.Permanent;
    }

    public AuthenticationValidationException(string message, Exception innerException) : base(message,
        innerException)
    {
        ResilienceErrorType = ResilienceErrorType.Permanent;
    }

    public AuthenticationValidationException(string message, ResilienceErrorType resilienceErrorType) : base(
        message, resilienceErrorType)
    {
        ResilienceErrorType = resilienceErrorType;
    }

    public AuthenticationValidationException(string message, IEnumerable<ValidationResult> validationResults)
        : base(message)
    {
        ResilienceErrorType = ResilienceErrorType.Permanent;
        Validations = validationResults;
    }

    public AuthenticationValidationException(string message, ResilienceErrorType resilienceErrorType,
        IEnumerable<ValidationResult> validationResults)
        : base(message)
    {
        ResilienceErrorType = resilienceErrorType;
        Validations = validationResults;
    }

    /// <summary>
    ///     Código do erro.
    /// </summary>
    public string Code { get; }

    /// <summary>
    ///     Contexto de validação.
    /// </summary>
    public IEnumerable<ValidationResult> Validations { get; }
}