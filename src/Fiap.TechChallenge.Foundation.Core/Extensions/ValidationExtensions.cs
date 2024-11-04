using System.Text.RegularExpressions;
using Fiap.TechChallenge.Foundation.Core.Validations;

namespace Fiap.TechChallenge.Foundation.Core.Extensions;

public static class ValidationExtensions
{
    /// <summary>
    ///     Valida se o e-mail fornecido tem um formato válido.
    /// </summary>
    /// <param name="contract">Objeto de validação que permite encadeamento de métodos.</param>
    /// <param name="email">O e-mail a ser validado.</param>
    /// <param name="propertyName">O nome da propriedade a ser validada.</param>
    /// <returns>O próprio objeto de validação para permitir encadeamento de validações.</returns>
    public static Contract IsValidEmail(this Contract contract, string email, string propertyName)
    {
        // Regex básico para validação de e-mail.
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (!Regex.IsMatch(email, emailPattern))
            throw new ArgumentException($"O campo {propertyName} não é um e-mail válido.");

        return contract;
    }


    /// <summary>
    ///     Valida se a string fornecida tem o comprimento exato esperado.
    /// </summary>
    /// <param name="contract">Objeto de validação que permite encadeamento de métodos.</param>
    /// <param name="value">A string a ser validada.</param>
    /// <param name="length">O comprimento exato esperado da string.</param>
    /// <param name="propertyName">O nome da propriedade a ser validada (usado para mensagens de erro).</param>
    /// <returns>O próprio objeto de validação para permitir encadeamento de validações.</returns>
    public static Contract HasLength(this Contract contract, string value, int length, string propertyName)
    {
        if (value.Length != length)
            throw new ArgumentException($"O campo {propertyName} deve ter exatamente {length} caracteres.");

        return contract;
    }
}