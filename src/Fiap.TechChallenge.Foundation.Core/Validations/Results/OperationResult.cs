using System.Text;

namespace Fiap.TechChallenge.Foundation.Core.Validations.Results;

/// <summary>
///     Resultado de uma operação (método) realizado.
/// </summary>
public sealed class OperationResult<TResult> : IOperationResult
{
    public OperationResult(TResult result)
    {
        Succeded = true;
        Result = result;
    }

    private OperationResult(IEnumerable<ValidationResult> validationResults)
    {
        Succeded = false;
        Validations = validationResults;
    }

    private OperationResult(string message)
    {
        Succeded = false;
        Validations = new List<ValidationResult> { new(message) };
    }

    private OperationResult(string code, string message)
    {
        Succeded = false;
        Validations = new List<ValidationResult> { new(code, message) };
    }

    /// <summary>
    ///     Resultado da Operação.
    /// </summary>
    public TResult Result { get; }

    /// <summary>
    ///     Indica que a operação foi realizada com sucesso.
    /// </summary>
    public bool Succeded { get; }

    /// <summary>
    ///     Lista de valicações realizadas.
    /// </summary>
    public IEnumerable<ValidationResult> Validations { get; }

    /// <summary>
    ///     Indica uma operação que finalizou com sucesso.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static OperationResult<TResult> Ok(TResult result)
    {
        return new OperationResult<TResult>(result);
    }

    /// <summary>
    ///     Indica uma operação que falhou.
    /// </summary>
    /// <param name="validations"></param>
    /// <returns></returns>
    public static OperationResult<TResult> Fail(IEnumerable<ValidationResult> validations)
    {
        return new OperationResult<TResult>(validations);
    }

    /// <summary>
    ///     Indica uma operação que falhou.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static OperationResult<TResult> Fail(string message)
    {
        return new OperationResult<TResult>(message);
    }

    /// <summary>
    ///     Indica uma operação que falhou.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static OperationResult<TResult> Fail(string code, string message)
    {
        return new OperationResult<TResult>(code, message);
    }

    public override string ToString()
    {
        if (Succeded)
            return "succeded";

        var sb = new StringBuilder();

        foreach (var validation in Validations)
            sb.AppendLine(validation.ToString());

        return sb.ToString();
    }
}