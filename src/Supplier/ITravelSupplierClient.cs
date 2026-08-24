namespace Webinar.Supplier;

/// <summary>
/// Stands in for whatever supplier SDK you actually use. Not the interesting part.
/// </summary>
public interface ITravelSupplierClient
{
    Task<string> CreateBookingAsync(string apiKey, string email, string companyName, CancellationToken ct);

    Task<bool> PingAsync(string apiKey, CancellationToken ct);
}
