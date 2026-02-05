
namespace PropertyViewings.Application.Features.Search
{
    public sealed record SearchAvailabilityResponse(
    string PropertyId,
    DateOnly From,
    DateOnly To,
    IReadOnlyList<DateTime> AvailableStartTimesUtc);
}
