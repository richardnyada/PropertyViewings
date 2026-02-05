namespace PropertyViewings.Domain.Time;

public static class UkTime
{
    public const string UkTimeZoneId = "Europe/London";

    public static TimeZoneInfo UkTimeZone { get; } = ResolveUkTimeZone();

    private static TimeZoneInfo ResolveUkTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(UkTimeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            // Windows
            return TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        }
        catch (InvalidTimeZoneException)
        {
            // Fallback to UTC if OS tz data is broken
            return TimeZoneInfo.Utc;
        }
    }

    public static DateTime ToUkLocal(DateTime input)
    {
        var utc = input.Kind switch
        {
            DateTimeKind.Utc => input,
            DateTimeKind.Local => input.ToUniversalTime(),
            // Legacy: treat unspecified clock value as UTC.
            DateTimeKind.Unspecified => DateTime.SpecifyKind(input, DateTimeKind.Utc),
            _ => DateTime.SpecifyKind(input, DateTimeKind.Utc)
        };

        return TimeZoneInfo.ConvertTimeFromUtc(utc, UkTimeZone);
    }

    public static DateTime ToUtcFromUkLocal(DateTime ukLocal)
    {
        if (ukLocal.Kind == DateTimeKind.Unspecified)
            return TimeZoneInfo.ConvertTimeToUtc(ukLocal, UkTimeZone);

        // Interpret the clock value as UK local regardless of kind.
        var unspecified = DateTime.SpecifyKind(ukLocal, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(unspecified, UkTimeZone);
    }

    public static DateTime StartOfUkDayUtc(DateOnly day)
    {
        var ukMidnightUnspecified = DateTime.SpecifyKind(day.ToDateTime(TimeOnly.MinValue), DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(ukMidnightUnspecified, UkTimeZone);
    }
}
