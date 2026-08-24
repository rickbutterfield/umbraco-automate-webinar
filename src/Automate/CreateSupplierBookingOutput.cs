namespace Webinar.Automate;

/// <summary>
/// The next step can bind to this. That is how a chain of steps passes data along.
/// </summary>
public sealed class CreateSupplierBookingOutput
{
    public required string BookingReference { get; init; }
}
