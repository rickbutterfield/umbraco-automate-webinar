using Umbraco.Cms.Core.Notifications;

namespace Webinar.Bookings;

/// <summary>
/// Raised by our own site code when a company submits a trip enquiry.
/// Nothing to do with Automate — this is a plain Umbraco notification.
/// </summary>
public class TripEnquirySubmittedNotification : INotification
{
    public required Guid EnquiryId { get; init; }

    public required string Email { get; init; }

    public required string CompanyName { get; init; }
}
