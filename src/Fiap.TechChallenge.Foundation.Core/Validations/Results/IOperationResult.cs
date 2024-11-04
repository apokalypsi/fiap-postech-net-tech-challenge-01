namespace Fiap.TechChallenge.Foundation.Core.Validations.Results;

/// <summary>
///     Resultado de uma operação (método) realizado.
/// </summary>
public interface IOperationResult
{
    bool Succeded { get; }
    IEnumerable<ValidationResult> Validations { get; }
}