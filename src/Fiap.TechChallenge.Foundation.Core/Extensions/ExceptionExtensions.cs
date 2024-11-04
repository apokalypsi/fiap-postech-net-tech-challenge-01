using System.Reflection;

namespace Fiap.TechChallenge.Foundation.Core.Extensions;

internal static class ExceptionExtensions
{
    public static Exception Unwrap(this TargetInvocationException exception)
    {
        Exception result = exception;
        while (result is TargetInvocationException) result = result.InnerException;

        return result;
    }
}