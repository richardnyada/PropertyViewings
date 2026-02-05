using FluentAssertions;
using PropertyViewings.Application.Features.Search;

namespace PropertyViewings.Tests.Search
{
    public class SlotGeneratorTests
    {
        [Fact]
        public void GenerateDaySlotsUtc_Gmt_GeneratesExpectedCountAndBoundaries()
        {
            var day = new DateOnly(2026, 02, 05);

            var slots = SlotGenerator.GenerateDaySlotsUtc(day).ToList();

            // 09:00 -> 19:30 inclusive, every 30 mins => 22 slots
            slots.Should().HaveCount(22);

            slots.First().Should().Be(new DateTime(2026, 02, 05, 09, 00, 00, DateTimeKind.Utc));
            slots.Last().Should().Be(new DateTime(2026, 02, 05, 19, 30, 00, DateTimeKind.Utc));
        }

        [Fact]
        public void GenerateDaySlotsUtc_Bst_ShiftsByOneHour_InUtc()
        {
            var day = new DateOnly(2026, 07, 01);

            var slots = SlotGenerator.GenerateDaySlotsUtc(day).ToList();

            slots.Should().HaveCount(22);

            // UK 09:00 is UTC 08:00 during BST
            slots.First().Should().Be(new DateTime(2026, 07, 01, 08, 00, 00, DateTimeKind.Utc));
            // UK 19:30 is UTC 18:30 during BST
            slots.Last().Should().Be(new DateTime(2026, 07, 01, 18, 30, 00, DateTimeKind.Utc));
        }
    }
}
