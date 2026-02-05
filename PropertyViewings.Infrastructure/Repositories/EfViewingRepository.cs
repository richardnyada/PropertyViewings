using Microsoft.EntityFrameworkCore;
using PropertyViewings.Application.Abstractions;
using PropertyViewings.Domain.Entities;
using PropertyViewings.Infrastructure.Data;

namespace PropertyViewings.Infrastructure.Repositories
{
    public class EfViewingRepository : IViewingRepository
    {
        private readonly AppDbContext _db;

        public EfViewingRepository(AppDbContext db) => _db = db;

        public Task<bool> SlotExistsAsync(string propertyId, DateTime startUtc, CancellationToken ct)
            => _db.Viewings.AnyAsync(v => v.PropertyId == propertyId && v.StartTimeUtc == startUtc, ct);

        public async Task AddAsync(ViewingBooking booking, CancellationToken ct)
        {
            _db.Viewings.Add(booking);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<DateTime>> GetBookedStartsAsync(
            string propertyId,
            DateTime fromUtcInclusive,
            DateTime toUtcExclusive,
            CancellationToken ct)
        {
            return await _db.Viewings
                .Where(v => v.PropertyId == propertyId
                            && v.StartTimeUtc >= fromUtcInclusive
                            && v.StartTimeUtc < toUtcExclusive)
                .Select(v => v.StartTimeUtc)
                .ToListAsync(ct);
        }
    }
}
