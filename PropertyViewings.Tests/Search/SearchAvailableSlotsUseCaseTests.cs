using FluentAssertions;
using PropertyViewings.Application.Features.Bookings;
using PropertyViewings.Application.Features.Search;
using PropertyViewings.Tests.TestDoubles;

namespace PropertyViewings.Tests.Search
{

    public class SearchAvailableSlotsUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_ReturnsAllSlotsExceptBooked_OnSingleDay()
        {
            // Arrange
            var repo = new InMemoryViewingRepository();

            // Seed bookings via the booking use case (so we validate the whole flow)
            var bookingValidator = new BookViewingValidator();
            var bookUseCase = new BookViewingUseCase(repo, bookingValidator);

            var day = new DateOnly(2026, 02, 05);
            var propertyId = "PROP-1";

            var s1 = new DateTime(2026, 02, 05, 09, 00, 00, DateTimeKind.Utc);
            var s2 = new DateTime(2026, 02, 05, 09, 30, 00, DateTimeKind.Utc);

            await bookUseCase.ExecuteAsync(new BookViewingRequest(propertyId, "USER-1", s1), CancellationToken.None);
            await bookUseCase.ExecuteAsync(new BookViewingRequest(propertyId, "USER-2", s2), CancellationToken.None);

            var searchUseCase = new SearchAvailableSlotsUseCase(repo);

            // Act
            var result = await searchUseCase.ExecuteAsync(propertyId, day, day, CancellationToken.None);

            // Assert
            result.PropertyId.Should().Be(propertyId);
            result.From.Should().Be(day);
            result.To.Should().Be(day);

            // Total daily slots: 22 (09:00 -> 19:30). We booked 2 => 20 available.
            result.AvailableStartTimesUtc.Should().HaveCount(20);

            result.AvailableStartTimesUtc.Should().NotContain(s1);
            result.AvailableStartTimesUtc.Should().NotContain(s2);

            // sanity: should still include later slots
            result.AvailableStartTimesUtc.Should().Contain(new DateTime(2026, 02, 05, 10, 00, 00, DateTimeKind.Utc));
            result.AvailableStartTimesUtc.Should().Contain(new DateTime(2026, 02, 05, 19, 30, 00, DateTimeKind.Utc));
        }
    }
}
