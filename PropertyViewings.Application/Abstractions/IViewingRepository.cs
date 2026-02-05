using PropertyViewings.Domain.Entities;

namespace PropertyViewings.Application.Abstractions
{
    public interface IViewingRepository
    {
        Task<bool> SlotExistsAsync(string propertyId, DateTime startUtc, CancellationToken ct);
        Task AddAsync(ViewingBooking booking, CancellationToken ct);

        Task<IReadOnlyList<DateTime>> GetBookedStartsAsync(
            string propertyId,
            DateTime fromUtcInclusive,
            DateTime toUtcExclusive,
            CancellationToken ct);
    }
}
