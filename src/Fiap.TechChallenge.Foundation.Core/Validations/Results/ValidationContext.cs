namespace Fiap.TechChallenge.Foundation.Core.Validations.Results;

public sealed class ValidationContext : IValidationContext
{
    private readonly List<ValidationResult> _results = new();

    public IEnumerable<ValidationResult> Results => _results;

    public void AddValidationResult(ValidationResult validationResult)
    {
        if (validationResult == null) throw new ArgumentNullException(nameof(validationResult));
        _results.Add(validationResult);
    }

    public void AddValidationResultCollection(IEnumerable<ValidationResult> validationsResult)
    {
        _results.AddRange(validationsResult);
    }
}