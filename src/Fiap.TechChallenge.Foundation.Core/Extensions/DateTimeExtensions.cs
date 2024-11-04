namespace Fiap.TechChallenge.Foundation.Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTime LastDayOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }

    public static DateTime FirstDayOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }

    public static IEnumerable<DateTime> GetAllMonthsUntil(this DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate) throw new ArgumentException("End date is greater than start date.");

        var halfStartDate = new DateTime(startDate.Year, startDate.Month, 15);
        var halfEndDate = new DateTime(endDate.Year, endDate.Month, 15);

        do
        {
            yield return halfStartDate.FirstDayOfMonth();
            halfStartDate = halfStartDate.AddMonths(1);
        } while (halfStartDate <= halfEndDate);
    }

    public static DateTime GetPreviousMonthPeriod(this DateTime value, DateTime startDate, DateTime endDate)
    {
        var monthsDiff = endDate.Subtract(startDate).Days / 30;

        if (monthsDiff == 0) monthsDiff = 1;

        return value.AddMonths(-monthsDiff);
    }

    public static DateTime FirstOfYear(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second,
            dateTime.Millisecond);
    }

    public static DateTime LastDayOfYear(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 12, 31, dateTime.Hour, dateTime.Minute, dateTime.Second,
            dateTime.Millisecond);
    }

    public static DateTime FirstMomentOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
    }

    public static DateTime LastMomentOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
    }
}