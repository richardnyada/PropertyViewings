using FluentValidation;
using PropertyViewings.Domain.Policies;

namespace PropertyViewings.Application.Features.Bookings
{
    public sealed class BookViewingValidator : AbstractValidator<BookViewingRequest>
    {
        public BookViewingValidator()
        {
            RuleFor(x => x.PropertyId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.StartTimeUtc)
                .Must(x => x.Kind == DateTimeKind.Utc || x.Kind == DateTimeKind.Unspecified)
                .WithMessage("StartTimeUtc must be UTC (or unspecified).");

            RuleFor(x => x.StartTimeUtc)
                .Must(BusinessHours.IsSlotAligned)
                .WithMessage("StartTimeUtc must align to 30-minute boundaries (e.g., 09:00, 09:30).");

            RuleFor(x => x.StartTimeUtc)
                .Must(BusinessHours.IsWithinHours)
                .WithMessage("StartTimeUtc must be within business hours 09:00-20:00 (UTC).");

            RuleFor(x => x.StartTimeUtc)
                .Must(BusinessHours.WouldEndWithinHours)
                .WithMessage("Slot must end by 20:00. Last valid start is 19:30.");
        }
    }
}
