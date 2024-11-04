namespace Fiap.TechChallenge.Foundation.Core.Validations.Results;

/// <summary>
///     Coleção das mensagens de validações.
/// </summary>
public interface IValidationContext
{
    IEnumerable<ValidationResult> Results { get; }
    void AddValidationResult(ValidationResult validationResult);
    void AddValidationResultCollection(IEnumerable<ValidationResult> validationsResult);
}