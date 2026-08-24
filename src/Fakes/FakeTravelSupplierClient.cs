using Microsoft.Extensions.Logging;
using Webinar.Supplier;

namespace Webinar.Fakes;

/// <summary>
/// Scaffolding, not part of the talk. Stands in for a real supplier SDK so the action has
/// something to call. Deliberately lives outside the folders prezl.yaml exposes.
/// </summary>
public sealed class FakeTravelSupplierClient : ITravelSupplierClient
{
    private readonly ILogger<FakeTravelSupplierClient> _logger;

    public FakeTravelSupplierClient(ILogger<FakeTravelSupplierClient> logger) => _logger = logger;

    public Task<string> CreateBookingAsync(string apiKey, string email, string companyName, CancellationToken ct)
    {
        var bookingReference = $"BOOK-{Guid.NewGuid().ToString()[..8].ToUpperInvariant()}";
        _logger.LogInformation("Fake supplier created {BookingReference} for {Email} at {Company}", bookingReference, email, companyName);
        return Task.FromResult(bookingReference);
    }

    public Task<bool> PingAsync(string apiKey, CancellationToken ct)
        // Any non-empty key is "valid", so the connection test goes green on stage.
        => Task.FromResult(!string.IsNullOrWhiteSpace(apiKey));
}
