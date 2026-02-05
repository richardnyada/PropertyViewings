using PropertyViewings.Domain.Policies;
using PropertyViewings.Domain.Time;

namespace PropertyViewings.Application.Features.Search
{
    public static class SlotGenerator
    {
        public static IEnumerable<DateTime> GenerateDaySlotsUtc(DateOnly day)
        {
            // 09:00 -> 19:30 (ends at 20:00), UK local time; returned as UTC
            var startUkLocalUnspecified = DateTime.SpecifyKind(day.ToDateTime(BusinessHours.Open), DateTimeKind.Unspecified);
            var lastStartUkLocalUnspecified = DateTime.SpecifyKind(day.ToDateTime(BusinessHours.Close), DateTimeKind.Unspecified)
                .AddMinutes(-BusinessHours.SlotMinutes);

            for (var t = startUkLocalUnspecified; t <= lastStartUkLocalUnspecified; t = t.AddMinutes(BusinessHours.SlotMinutes))
                yield return UkTime.ToUtcFromUkLocal(t);
        }
    }
}
