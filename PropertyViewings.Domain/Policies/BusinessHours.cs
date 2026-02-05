
namespace PropertyViewings.Domain.Policies
{
    public static class BusinessHours
    {
        public static readonly TimeOnly Open = new(9, 0);
        public static readonly TimeOnly Close = new(20, 0);
        public const int SlotMinutes = 30;

        public static bool IsSlotAligned(DateTime startUtc) =>
            startUtc.Second == 0 &&
            startUtc.Millisecond == 0 &&
            (startUtc.Minute % SlotMinutes == 0);

        public static bool IsWithinHours(DateTime startUtc)
        {
            var t = TimeOnly.FromDateTime(startUtc);
            // start must be >= 09:00 and < 20:00
            return t >= Open && t < Close;
        }

        public static DateTime EndTimeUtc(DateTime startUtc) => startUtc.AddMinutes(SlotMinutes);

        public static bool WouldEndWithinHours(DateTime startUtc)
        {
            // End must be <= 20:00 on the same day
            var closeUtc = DateTime.SpecifyKind(startUtc.Date, DateTimeKind.Utc)
                .Add(Close.ToTimeSpan());

            return EndTimeUtc(startUtc) <= closeUtc;
        }
    }
}
