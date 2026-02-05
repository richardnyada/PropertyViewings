using Microsoft.AspNetCore.Mvc;
using PropertyViewings.Application.Features.Bookings;
using PropertyViewings.Application.Features.Search;

namespace PropertyViewings.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class ViewingsController : ControllerBase
    {
        [HttpPost("viewings")]
        public async Task<ActionResult<BookViewingResponse>> Book(
            [FromBody] BookViewingRequest request,
            [FromServices] BookViewingUseCase useCase,
            CancellationToken ct)
        {
            var result = await useCase.ExecuteAsync(request, ct);
            return Created($"/api/viewings/{result.BookingId}", result);
        }

        // GET /api/properties/{propertyId}/availability?from=2026-02-05&to=2026-02-06
        [HttpGet("properties/{propertyId}/availability")]
        public async Task<ActionResult<SearchAvailabilityResponse>> Availability(
            [FromRoute] string propertyId,
            [FromQuery] DateOnly from,
            [FromQuery] DateOnly to,
            [FromServices] SearchAvailableSlotsUseCase useCase,
            CancellationToken ct)
        {
            var result = await useCase.ExecuteAsync(propertyId, from, to, ct);
            return Ok(result);
        }
    }
}
