using FluentAssertions;
using FluentValidation;
using PropertyViewings.Application.Features.Bookings;
using PropertyViewings.Tests.TestDoubles;

namespace PropertyViewings.Tests.Bookings
{
    public class BookViewingUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_BooksSuccessfully_WhenSlotIsFree()
        {
            var repo = new InMemoryViewingRepository();
            var validator = new BookViewingValidator();
            var useCase = new BookViewingUseCase(repo, validator);

            var request = new BookViewingRequest("PROP-1", "USER-1", new DateTime(2026, 02, 05, 09, 00, 00, DateTimeKind.Utc));

            var result = await useCase.ExecuteAsync(request, CancellationToken.None);

            result.PropertyId.Should().Be("PROP-1");
            result.UserId.Should().Be("USER-1");
            result.StartTimeUtc.Should().Be(new DateTime(2026, 02, 05, 09, 00, 00, DateTimeKind.Utc));
            result.BookingId.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsConflict_WhenSlotAlreadyBooked()
        {
            var repo = new InMemoryViewingRepository();
            var validator = new BookViewingValidator();
            var useCase = new BookViewingUseCase(repo, validator);

            var start = new DateTime(2026, 02, 05, 09, 30, 00, DateTimeKind.Utc);
            var request1 = new BookViewingRequest("PROP-1", "USER-1", start);
            var request2 = new BookViewingRequest("PROP-1", "USER-2", start);

            await useCase.ExecuteAsync(request1, CancellationToken.None);

            Func<Task> act = async () => await useCase.ExecuteAsync(request2, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Slot is already booked for this property.");
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsValidationException_WhenTimeNotAligned()
        {
            var repo = new InMemoryViewingRepository();
            var validator = new BookViewingValidator();
            var useCase = new BookViewingUseCase(repo, validator);

            var request = new BookViewingRequest("PROP-1", "USER-1", new DateTime(2026, 02, 05, 09, 15, 00, DateTimeKind.Utc));

            Func<Task> act = async () => await useCase.ExecuteAsync(request, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsValidationException_WhenOutsideBusinessHours()
        {
            var repo = new InMemoryViewingRepository();
            var validator = new BookViewingValidator();
            var useCase = new BookViewingUseCase(repo, validator);

            var request = new BookViewingRequest("PROP-1", "USER-1", new DateTime(2026, 02, 05, 20, 00, 00, DateTimeKind.Utc));

            Func<Task> act = async () => await useCase.ExecuteAsync(request, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
