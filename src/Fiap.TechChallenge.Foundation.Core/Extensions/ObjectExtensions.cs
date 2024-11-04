using System.ComponentModel;
using System.Globalization;

namespace Fiap.TechChallenge.Foundation.Core.Extensions;

public static class ObjectExtensions
{
    public static bool IsSameOrEqualTo(this object actual, object expected)
    {
        if (actual is null && expected is null) return true;

        if (actual is null) return false;

        if (expected is null) return false;

        if (actual.Equals(expected)) return true;

        try
        {
            if (!(expected is string) && !(actual is string))
            {
                var convertedActual = Convert.ChangeType(actual, expected.GetType());
                return convertedActual.Equals(expected);
            }
        }
        catch
        {
            // ignored
        }

        return false;
    }

    /// <summary>
    ///     Used to simplify and beautify casting an object to a type.
    /// </summary>
    /// <typeparam name="T">Type to be casted</typeparam>
    /// <param name="obj">Object to cast</param>
    /// <returns>Casted object</returns>
    public static T As<T>(this object obj)
        where T : class
    {
        return (T)obj;
    }

    /// <summary>
    ///     Converts given object to a value type using <see cref="Convert.ChangeType(object,System.TypeCode)" /> method.
    /// </summary>
    /// <param name="obj">Object to be converted</param>
    /// <typeparam name="T">Type of the target object</typeparam>
    /// <returns>Converted object</returns>
    public static T To<T>(this object obj)
        where T : struct
    {
        if (typeof(T) == typeof(Guid))
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());

        return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Check if an item is in a list.
    /// </summary>
    /// <param name="item">Item to check</param>
    /// <param name="list">List of items</param>
    /// <typeparam name="T">Type of the items</typeparam>
    public static bool IsIn<T>(this T item, params T[] list)
    {
        return list.Contains(item);
    }
}