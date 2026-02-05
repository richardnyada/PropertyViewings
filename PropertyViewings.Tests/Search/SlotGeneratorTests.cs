using FluentAssertions;
using PropertyViewings.Application.Features.Search;

namespace PropertyViewings.Tests.Search
{
    public class SlotGeneratorTests
    {
        [Fact]
        public void GenerateDaySlotsUtc_GeneratesExpectedCountAndBoundaries()
        {
            var day = new DateOnly(2026, 02, 05);

            var slots = SlotGenerator.GenerateDaySlotsUtc(day).ToList();

            // 09:00 -> 19:30 inclusive, every 30 mins => 22 slots
            slots.Should().HaveCount(22);

            slots.First().Should().Be(new DateTime(2026, 02, 05, 09, 00, 00, DateTimeKind.Utc));
            slots.Last().Should().Be(new DateTime(2026, 02, 05, 19, 30, 00, DateTimeKind.Utc));
        }
    }
}
