using PropertyViewings.Domain.Policies;

namespace PropertyViewings.Application.Features.Search
{
    public static class SlotGenerator
    {
        public static IEnumerable<DateTime> GenerateDaySlotsUtc(DateOnly day)
        {
            // 09:00 -> 19:30 (ends at 20:00)
            var start = DateTime.SpecifyKind(day.ToDateTime(BusinessHours.Open), DateTimeKind.Utc);
            var lastStart = DateTime.SpecifyKind(day.ToDateTime(BusinessHours.Close), DateTimeKind.Utc)
                .AddMinutes(-BusinessHours.SlotMinutes);

            for (var t = start; t <= lastStart; t = t.AddMinutes(BusinessHours.SlotMinutes))
                yield return t;
        }
    }
}
