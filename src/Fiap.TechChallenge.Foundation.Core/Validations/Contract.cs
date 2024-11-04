using System.ComponentModel.DataAnnotations;
using System.Text;
using Fiap.TechChallenge.Foundation.Core.Exceptions;
using Fiap.TechChallenge.Foundation.Core.Extensions;
using Fiap.TechChallenge.Foundation.Core.Languages;
using Fiap.TechChallenge.Foundation.Core.Validations.Results;

namespace Fiap.TechChallenge.Foundation.Core.Validations;

public partial class Contract : ContractBase
{
    public static Contract Requires => new();


    /// <summary>
    ///     Lança BusinessException para validações de negócio.
    /// </summary>
    /// <exception cref="BusinessException"></exception>
    public void Guard()
    {
        Guard(Resources.COR_002);
    }

    /// <summary>
    ///     Lança BusinessException para validações de negócio.
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="BusinessException"></exception>
    public void Guard(string message)
    {
        if (Valid) return;
        throw new BusinessException(message, this);
    }

    /// <summary>
    ///     Lança ValidationException para validações de negócio.
    /// </summary>
    /// <exception cref="ValidationException"></exception>
    public void GuardValidation()
    {
        if (Valid) return;
        var sbrErrors = new StringBuilder();
        var results = Validations;
        foreach (var result in results) sbrErrors.AppendLine(result.Message);
        throw new ValidationException(sbrErrors.ToString());
    }

    /// <summary>
    ///     Gera um resultado de Operação baseado nas valdações aplicadas.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public OperationResult<TResult> Result<TResult>(TResult result)
    {
        if (result != null && Valid) return OperationResult<TResult>.Ok(result);
        return OperationResult<TResult>.Fail(Validations);
    }

    /// <summary>
    ///     Método para permitir composição de validadores.
    /// </summary>
    /// <param name="validatableObject">Objeto a ser validado.</param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public Contract IsValid(IValidatable validatableObject, string propertyName)
    {
        if (validatableObject == null)
        {
            AddValidation(Resources.COR_001.Args(propertyName));
            return this;
        }

        AddValidations(validatableObject.Validation().Validations);
        return this;
    }

    public Contract IsTrue(bool isValid, string property, string message)
    {
        if (!isValid) AddValidation(property, message);
        return this;
    }

    public Contract IsTrue(bool isValid, string message)
    {
        if (!isValid) AddValidation(message);
        return this;
    }

    public Contract IsFalse(bool isValid, string message)
    {
        if (isValid) AddValidation(message);
        return this;
    }

    public Contract IsFalse(bool isValid, string property, string message)
    {
        if (isValid) AddValidation(property, message);
        return this;
    }
}