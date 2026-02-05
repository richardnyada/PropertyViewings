
namespace PropertyViewings.Application.Features.Bookings
{
    public sealed record BookViewingResponse(Guid BookingId, string PropertyId, string UserId, DateTime StartTimeUtc);
}
