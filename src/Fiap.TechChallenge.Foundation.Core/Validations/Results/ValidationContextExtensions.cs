using Fiap.TechChallenge.Foundation.Core.Exceptions;
using Fiap.TechChallenge.Foundation.Core.Languages;

namespace Fiap.TechChallenge.Foundation.Core.Validations.Results;

public static class ValidationContextExtensions
{
    public static void LoadFromValidations(this IValidationContext target, ContractBase validatable)
    {
        AddValidations(target, validatable);
    }

    public static void AddValidations(this IValidationContext target, ContractBase validatable)
    {
        if (validatable == null) return;
        target.AddValidationResultCollection(validatable.Validations);
    }

    public static void Guard(this IValidationContext target)
    {
        if (!target.Results.Any()) return;
        throw new BusinessException(Resources.COR_002, target);
    }

    public static void Guard(this IValidationContext target, string message)
    {
        if (!target.Results.Any()) return;
        throw new BusinessException(message, target);
    }
}