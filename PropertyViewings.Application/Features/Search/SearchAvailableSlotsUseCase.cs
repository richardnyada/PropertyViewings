using PropertyViewings.Application.Abstractions;

namespace PropertyViewings.Application.Features.Search
{
    public sealed class SearchAvailableSlotsUseCase
    {
        private readonly IViewingRepository _repo;

        public SearchAvailableSlotsUseCase(IViewingRepository repo) => _repo = repo;

        public async Task<SearchAvailabilityResponse> ExecuteAsync(string propertyId, DateOnly from, DateOnly to, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(propertyId))
                throw new InvalidOperationException("PropertyId is required.");

            if (to < from)
                throw new InvalidOperationException("To must be on or after From.");

            var fromUtc = DateTime.SpecifyKind(from.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
            var toUtcExclusive = DateTime.SpecifyKind(to.AddDays(1).ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

            var booked = await _repo.GetBookedStartsAsync(propertyId, fromUtc, toUtcExclusive, ct);
            var bookedSet = booked.ToHashSet();

            var available = new List<DateTime>();

            for (var day = from; day <= to; day = day.AddDays(1))
            {
                foreach (var slotStart in SlotGenerator.GenerateDaySlotsUtc(day))
                {
                    if (!bookedSet.Contains(slotStart))
                        available.Add(slotStart);
                }
            }

            return new SearchAvailabilityResponse(propertyId, from, to, available);
        }
    }
}
