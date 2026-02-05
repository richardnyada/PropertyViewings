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
        public void IsWithinHours_True_InsideBusinessHours_Gmt()
        {
            // Feb is typically GMT (UTC+0)
            BusinessHours.IsWithinHours(new DateTime(2026, 02, 05, 09, 00, 00, DateTimeKind.Utc)).Should().BeTrue();
            BusinessHours.IsWithinHours(new DateTime(2026, 02, 05, 19, 59, 00, DateTimeKind.Utc)).Should().BeTrue();
        }

        [Fact]
        public void IsWithinHours_False_AtOrAfterClose_Gmt()
        {
            BusinessHours.IsWithinHours(new DateTime(2026, 02, 05, 20, 00, 00, DateTimeKind.Utc)).Should().BeFalse();
            BusinessHours.IsWithinHours(new DateTime(2026, 02, 05, 21, 00, 00, DateTimeKind.Utc)).Should().BeFalse();
        }

        [Fact]
        public void IsWithinHours_Bst_ShiftsByOneHour_InUtc()
        {
            // July is typically BST (UTC+1), so 09:00 UK == 08:00 UTC
            BusinessHours.IsWithinHours(new DateTime(2026, 07, 01, 08, 00, 00, DateTimeKind.Utc)).Should().BeTrue();
            BusinessHours.IsWithinHours(new DateTime(2026, 07, 01, 18, 30, 00, DateTimeKind.Utc)).Should().BeTrue(); // 19:30 UK
            BusinessHours.IsWithinHours(new DateTime(2026, 07, 01, 19, 00, 00, DateTimeKind.Utc)).Should().BeFalse(); // 20:00 UK
        }

        [Fact]
        public void WouldEndWithinHours_LastSlotAt1930_IsAllowed_Gmt()
        {
            BusinessHours.WouldEndWithinHours(new DateTime(2026, 02, 05, 19, 30, 00, DateTimeKind.Utc)).Should().BeTrue();
        }

        [Fact]
        public void WouldEndWithinHours_SlotStartingAt2000_IsNotAllowed_Gmt()
        {
            BusinessHours.WouldEndWithinHours(new DateTime(2026, 02, 05, 20, 00, 00, DateTimeKind.Utc)).Should().BeFalse();
        }
    }
}
