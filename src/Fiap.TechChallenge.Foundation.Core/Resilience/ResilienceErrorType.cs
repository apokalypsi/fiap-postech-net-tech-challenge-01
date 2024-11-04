namespace Fiap.TechChallenge.Foundation.Core.Resilience;

/// <summary>
///     Classificação básica de erros da aplicação referente a capacidade de reabilitação.
/// </summary>
public enum ResilienceErrorType
{
    /// <summary>
    ///     Indica um erro com capacidade de reabilitação automática.
    /// </summary>
    Transient = 1,

    /// <summary>
    ///     Indica um erro sem capacidade de reabilitação automática.
    /// </summary>
    Permanent = 2
}