using PropertyViewings.Application.Abstractions;
using PropertyViewings.Domain.Entities;
using System.Collections.Concurrent;

namespace PropertyViewings.Tests.TestDoubles
{
    public sealed class InMemoryViewingRepository : IViewingRepository
    {
        // Key: (propertyId, startUtc)
        private readonly ConcurrentDictionary<(string PropertyId, DateTime StartUtc), ViewingBooking> _store = new();

        public Task<bool> SlotExistsAsync(string propertyId, DateTime startUtc, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            return Task.FromResult(_store.ContainsKey((propertyId, startUtc)));
        }

        public Task AddAsync(ViewingBooking booking, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var key = (booking.PropertyId, booking.StartTimeUtc);
            if (!_store.TryAdd(key, booking))
                throw new InvalidOperationException("Slot is already booked for this property."); // mimic conflict

            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<DateTime>> GetBookedStartsAsync(
            string propertyId,
            DateTime fromUtcInclusive,
            DateTime toUtcExclusive,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var results = _store.Values
                .Where(v => v.PropertyId == propertyId
                            && v.StartTimeUtc >= fromUtcInclusive
                            && v.StartTimeUtc < toUtcExclusive)
                .Select(v => v.StartTimeUtc)
                .OrderBy(x => x)
                .ToList();

            return Task.FromResult<IReadOnlyList<DateTime>>(results);
        }
    }
}
