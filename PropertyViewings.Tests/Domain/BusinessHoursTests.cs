using FluentAssertions;
using PropertyViewings.Domain.Policies;

namespace PropertyViewings.Tests.Domain
{
    public class BusinessHoursTests
    {
        [Fact]
        public void IsSlotAligned_True_OnHalfHourBoundaries()
        {
            BusinessHours.IsSlotAligned(new DateTime(2026, 02, 05, 09, 00, 00, DateTimeKind.Utc)).Should().BeTrue();
            BusinessHours.IsSlotAligned(new DateTime(2026, 02, 05, 09, 30, 00, DateTimeKind.Utc)).Should().BeTrue();
            BusinessHours.IsSlotAligned(new DateTime(2026, 02, 05, 19, 30, 00, DateTimeKind.Utc)).Should().BeTrue();
        }

        [Fact]
        public void IsSlotAligned_False_OnNonAlignedMinutes()
        {
            BusinessHours.IsSlotAligned(new DateTime(2026, 02, 05, 09, 15, 00, DateTimeKind.Utc)).Should().BeFalse();
            BusinessHours.IsSlotAligned(new DateTime(2026, 02, 05, 09, 01, 00, DateTimeKind.Utc)).Should().BeFalse();
        }

        [Fact]
        public void IsWithinHours_True_InsideBusinessHours()
        {
            BusinessHours.IsWithinHours(new DateTime(2026, 02, 05, 09, 00, 00, DateTimeKind.Utc)).Should().BeTrue();
            BusinessHours.IsWithinHours(new DateTime(2026, 02, 05, 19, 59, 00, DateTimeKind.Utc)).Should().BeTrue();
        }

        [Fact]
        public void IsWithinHours_False_AtOrAfterClose()
        {
            BusinessHours.IsWithinHours(new DateTime(2026, 02, 05, 20, 00, 00, DateTimeKind.Utc)).Should().BeFalse();
            BusinessHours.IsWithinHours(new DateTime(2026, 02, 05, 21, 00, 00, DateTimeKind.Utc)).Should().BeFalse();
        }

        [Fact]
        public void WouldEndWithinHours_LastSlotAt1930_IsAllowed()
        {
            BusinessHours.WouldEndWithinHours(new DateTime(2026, 02, 05, 19, 30, 00, DateTimeKind.Utc)).Should().BeTrue();
        }

        [Fact]
        public void WouldEndWithinHours_SlotStartingAt2000_IsNotAllowed()
        {
            BusinessHours.WouldEndWithinHours(new DateTime(2026, 02, 05, 20, 00, 00, DateTimeKind.Utc)).Should().BeFalse();
        }
    }

}
