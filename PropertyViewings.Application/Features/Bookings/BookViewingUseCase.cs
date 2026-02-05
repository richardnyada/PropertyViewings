using FluentValidation;
using PropertyViewings.Application.Abstractions;
using PropertyViewings.Domain.Entities;

namespace PropertyViewings.Application.Features.Bookings
{
    public sealed class BookViewingUseCase
    {
        private readonly IViewingRepository _repo;
        private readonly IValidator<BookViewingRequest> _validator;

        public BookViewingUseCase(IViewingRepository repo, IValidator<BookViewingRequest> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        public async Task<BookViewingResponse> ExecuteAsync(BookViewingRequest request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var startUtc = DateTime.SpecifyKind(request.StartTimeUtc, DateTimeKind.Utc);

            if (await _repo.SlotExistsAsync(request.PropertyId, startUtc, ct))
                throw new InvalidOperationException("Slot is already booked for this property.");

            var booking = new ViewingBooking
            {
                PropertyId = request.PropertyId,
                UserId = request.UserId,
                StartTimeUtc = startUtc
            };

            await _repo.AddAsync(booking, ct);

            return new BookViewingResponse(booking.Id, booking.PropertyId, booking.UserId, booking.StartTimeUtc);
        }
    }
}
