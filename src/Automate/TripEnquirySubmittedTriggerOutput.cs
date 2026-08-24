namespace Webinar.Automate;

/// <summary>
/// Whatever we put here, every later step in the automation can bind to.
/// </summary>
public sealed class TripEnquirySubmittedTriggerOutput
{
    public required Guid EnquiryId { get; init; }

    public required string Email { get; init; }

    public required string CompanyName { get; init; }
}
