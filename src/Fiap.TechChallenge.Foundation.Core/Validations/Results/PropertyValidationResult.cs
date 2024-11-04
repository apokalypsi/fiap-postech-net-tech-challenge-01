namespace Fiap.TechChallenge.Foundation.Core.Validations.Results;

/// <inheritdoc />
/// <summary>
///     Representa o resultado de uma validação de propriedade.
/// </summary>
public sealed class PropertyValidationResult : ValidationResult
{
    public PropertyValidationResult(string propertyName, string message) : base(message)
    {
        PropertyName = propertyName;
    }

    public PropertyValidationResult(string propertyName, string code, string message) : base(code, message)
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; }

    public override string GetPropertyName()
    {
        return PropertyName;
    }

    public static PropertyValidationResult PropertyError(string propertyName, string message)
    {
        return new PropertyValidationResult(propertyName, message);
    }

    public static PropertyValidationResult PropertyError(string propertyName, string code, string message)
    {
        return new PropertyValidationResult(propertyName, code, message);
    }

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Code)) return $"{PropertyName}-{Message}";
        return $"{Code}-{PropertyName}-{Message}";
    }
}