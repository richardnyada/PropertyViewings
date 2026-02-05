
namespace PropertyViewings.Application.Features.Bookings
{
    public sealed record BookViewingRequest(string PropertyId, string UserId, DateTime StartTimeUtc);
}
