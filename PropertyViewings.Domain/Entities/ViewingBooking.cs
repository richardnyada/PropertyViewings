
namespace PropertyViewings.Domain.Entities;

public class ViewingBooking
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string PropertyId { get; set; }
    public required string UserId { get; set; }

    // Store as UTC
    public DateTime StartTimeUtc { get; set; }

}