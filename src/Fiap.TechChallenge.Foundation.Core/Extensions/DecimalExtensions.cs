namespace Fiap.TechChallenge.Foundation.Core.Extensions;

public static class DecimalExtensions
{
    public static decimal Percentage(this decimal value, decimal percentage)
    {
        return value * (percentage / 100);
    }

    public static decimal? Percentage(this decimal? value, decimal percentage)
    {
        if (value.HasValue) return Percentage(value.Value, percentage);

        return null;
    }

    public static decimal RoundTo(this decimal value, int decimals)
    {
        return Math.Round(value, decimals);
    }

    public static decimal? RoundTo(this decimal? value, int decimals)
    {
        if (value.HasValue) return RoundTo(value.Value, decimals);

        return null;
    }
}