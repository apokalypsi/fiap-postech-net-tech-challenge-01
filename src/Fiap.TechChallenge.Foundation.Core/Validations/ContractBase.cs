using Fiap.TechChallenge.Foundation.Core.Validations.Results;

namespace Fiap.TechChallenge.Foundation.Core.Validations;

public abstract class ContractBase
{
    private readonly List<ValidationResult> _validations = new();

    public IEnumerable<ValidationResult> Validations => _validations;

    public bool Invalid => Validations.Any();

    public bool Valid => !Invalid;

    protected void AddValidation(string message)
    {
        if (message.Contains('|'))
        {
            var messageWithCode = message.Split('|');
            _validations.Add(ValidationResult.Error(messageWithCode[0], messageWithCode[1]));
            return;
        }

        _validations.Add(ValidationResult.Error(message));
    }

    protected void AddValidation(string property, string message)
    {
        if (message.Contains('|'))
        {
            var messageWithCode = message.Split('|');
            _validations.Add(
                PropertyValidationResult.PropertyError(property, messageWithCode[0], messageWithCode[1]));
            return;
        }

        _validations.Add(PropertyValidationResult.PropertyError(property, message));
    }

    protected void AddValidation(PropertyValidationResult validationResult)
    {
        _validations.Add(validationResult);
    }

    protected void AddValidation(ValidationResult validationResult)
    {
        _validations.Add(validationResult);
    }

    protected void AddValidations(IEnumerable<ValidationResult> validations)
    {
        _validations.AddRange(validations);
    }
}