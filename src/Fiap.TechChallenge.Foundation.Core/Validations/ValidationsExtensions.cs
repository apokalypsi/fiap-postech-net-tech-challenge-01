using Fiap.TechChallenge.Foundation.Core.Injector;
using Fiap.TechChallenge.Foundation.Core.Validations.Results;

namespace Fiap.TechChallenge.Foundation.Core.Validations;

public static class ValidationsExtensions
{
    public static IInjector UseValidations(this IInjector injector)
    {
        injector.AddScoped<IValidationContext, ValidationContext>();
        return injector;
    }
}