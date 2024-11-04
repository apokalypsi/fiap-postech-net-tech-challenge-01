using Fiap.TechChallenge.Foundation.Core.Resilience;
using Fiap.TechChallenge.Foundation.Core.Validations;
using Fiap.TechChallenge.Foundation.Core.Validations.Results;

namespace Fiap.TechChallenge.Foundation.Core.Exceptions;

/// <summary>
///     Erros de negócio em geral.
/// </summary>
public sealed class BusinessException : ExceptionBaseWithResilienceType
{
    public BusinessException(string message)
        : base(message)
    {
        ResilienceErrorType = ResilienceErrorType.Permanent;
        Validations = new[] { ValidationResult.Error(message) };
    }

    public BusinessException(string message, ResilienceErrorType resilienceErrorType)
        : base(message)
    {
        ResilienceErrorType = resilienceErrorType;
        Validations = new[] { ValidationResult.Error(message) };
    }

    public BusinessException(string code, string message)
        : base(message)
    {
        Code = code;
        ResilienceErrorType = ResilienceErrorType.Permanent;
        Validations = new[] { ValidationResult.Error(code, message) };
    }

    public BusinessException(string code, string message, ResilienceErrorType resilienceErrorType)
        : base(message)
    {
        Code = code;
        ResilienceErrorType = ResilienceErrorType.Permanent;
        Validations = new[] { ValidationResult.Error(code, message) };
    }

    public BusinessException(string message, IValidationContext validationsResult)
        : base(message)
    {
        ResilienceErrorType = ResilienceErrorType.Permanent;
        Validations = validationsResult.Results;
    }

    public BusinessException(string message, Contract validate)
        : base(message)
    {
        ResilienceErrorType = ResilienceErrorType.Permanent;
        Validations = validate.Validations;
    }

    public BusinessException(string message, IEnumerable<ValidationResult> validationResults)
        : base(message)
    {
        ResilienceErrorType = ResilienceErrorType.Permanent;
        Validations = validationResults;
    }

    public BusinessException(string message, Exception innerException) : base(message, innerException)
    {
        ResilienceErrorType = ResilienceErrorType.Permanent;
        Validations = new[] { ValidationResult.Error(message) };
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