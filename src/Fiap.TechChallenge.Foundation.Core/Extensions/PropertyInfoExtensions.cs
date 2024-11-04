using System.Reflection;

namespace Fiap.TechChallenge.Foundation.Core.Extensions;

public static class PropertyInfoExtensions
{
    internal static bool IsVirtual(this PropertyInfo property)
    {
        var methodInfo = property.GetGetMethod(true) ?? property.GetSetMethod(true);
        return !methodInfo.IsNonVirtual();
    }
}