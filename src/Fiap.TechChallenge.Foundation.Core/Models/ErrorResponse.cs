using System.Text;
using Fiap.TechChallenge.Foundation.Core.Exceptions;
using Fiap.TechChallenge.Foundation.Core.Languages;
using Fiap.TechChallenge.Foundation.Core.Validations.Results;

namespace Fiap.TechChallenge.Foundation.Core.Models;

public class ErrorResponse
{
    public ErrorResponse(string message)
    {
        Code = Resources.COR_015;
        Message = message;
    }

    public ErrorResponse(string code, string message)
    {
        Code = string.IsNullOrWhiteSpace(code) ? Resources.COR_015 : code;
        Message = message;
    }

    public ErrorResponse(string message, IEnumerable<ValidationResult> details)
    {
        Code = Resources.COR_015;
        Message = message;
        Details = details;
    }

    public ErrorResponse(string code, string message, IEnumerable<ValidationResult> details)
    {
        Code = string.IsNullOrWhiteSpace(code) ? Resources.COR_015 : code;
        Message = message;
        Details = details;
    }

    public ErrorResponse(BusinessException businessException)
    {
        Code = string.IsNullOrWhiteSpace(businessException.Code) ? Resources.COR_015 : businessException.Code;
        Message = businessException.Message;
        Details = businessException.Validations;
    }

    public ErrorResponse(AuthenticationValidationException authenticationException)
    {
        Code = string.IsNullOrWhiteSpace(authenticationException.Code)
            ? Resources.COR_015
            : authenticationException.Code;
        Message = authenticationException.Message;
        Details = authenticationException.Validations;
    }

    public ErrorResponse(Exception genericException)
    {
        Code = Resources.COR_015;
        Message = Resources.API_001;
        Details = new List<ValidationResult> { ValidationResult.Error(genericException.Message) };
    }

    public ErrorResponse(string message, Exception genericException)
    {
        Code = Resources.COR_015;
        Message = message;
        Details = new List<ValidationResult> { ValidationResult.Error(genericException.Message) };
    }

    public ErrorResponse(string code, string message, Exception genericException)
    {
        Code = string.IsNullOrWhiteSpace(code) ? Resources.COR_015 : code;
        Message = message;
        Details = new List<ValidationResult> { ValidationResult.Error(genericException.Message) };
    }

    public string Code { get; set; }
    public string Message { get; set; }
    public IEnumerable<ValidationResult> Details { get; }

    public override string ToString()
    {
        var builder = new StringBuilder(Message);
        if (!Details.Any()) return builder.ToString();
        builder.Append("-Details:");
        foreach (var errorDetail in Details) builder.Append(errorDetail);

        return builder.ToString();
    }
}