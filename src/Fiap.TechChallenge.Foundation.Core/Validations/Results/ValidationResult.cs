namespace Fiap.TechChallenge.Foundation.Core.Validations.Results;

public class ValidationResult
{
    public ValidationResult(string message)
    {
        Message = message;
    }

    public ValidationResult(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    public virtual string GetPropertyName()
    {
        return "Context";
    }

    public static ValidationResult Error(string message)
    {
        return new ValidationResult(message);
    }

    public static ValidationResult Error(string code, string message)
    {
        return new ValidationResult(code, message);
    }

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Code)) return $"{Message}";
        return $"{Code}-{Message}";
    }
}