namespace Fiap.TechChallenge.Foundation.Core.Extensions;

public static class EnumerableExtensions
{
    public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
    {
        if (source == null) throw new ArgumentNullException("source");

        if (source.Contains(item)) return false;

        source.Add(item);
        return true;
    }

    /// <summary>
    ///     Filters elements that are different from null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<T> WhereElementNotNull<T>(this IEnumerable<T> source)
    {
        if (source == null) throw new ArgumentNullException("source");

        return source.Where(s => s != null);
    }

    public static decimal? TryMin<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
    {
        try
        {
            return source.Min(selector);
        }
        catch
        {
            return null;
        }
    }

    public static bool IsNullOrEmpty<T>(this ICollection<T> source)
    {
        return source == null || source.Count <= 0;
    }

    public static IEnumerable<TSource> FromHierarchy<TSource>(
        this TSource source,
        Func<TSource, TSource> nextItem,
        Func<TSource, bool> canContinue)
    {
        for (var current = source; canContinue(current); current = nextItem(current)) yield return current;
    }

    public static IEnumerable<TSource> FromHierarchy<TSource>(
        this TSource source,
        Func<TSource, TSource> nextItem)
        where TSource : class
    {
        return FromHierarchy(source, nextItem, s => s != null);
    }

    public static decimal TryAverage<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
    {
        try
        {
            return source.Average(selector);
        }
        catch (OverflowException)
        {
            return decimal.MaxValue;
        }
        catch
        {
            return 0;
        }
    }

    public static double TryAverage<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
    {
        try
        {
            return source.Average(selector);
        }
        catch (OverflowException)
        {
            return double.MaxValue;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    ///     Filters a <see cref="IEnumerable{T}" /> by given predicate if given condition is true.
    /// </summary>
    /// <param name="source">Enumerable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the enumerable</param>
    /// <returns>Filtered or not filtered enumerable based on <paramref name="condition" /></returns>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
    {
        return condition
            ? source.Where(predicate)
            : source;
    }
}