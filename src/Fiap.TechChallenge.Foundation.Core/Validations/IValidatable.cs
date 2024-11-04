namespace Fiap.TechChallenge.Foundation.Core.Validations;

/// <summary>
///     Indica um objeto auto validável.
/// </summary>
public interface IValidatable
{
    /// <summary>
    ///     Método que retorna uma validação.
    /// </summary>
    /// <returns></returns>
    Contract Validation();
}