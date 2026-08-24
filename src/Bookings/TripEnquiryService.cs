using Umbraco.Cms.Core.Events;

namespace Webinar.Bookings;

/// <summary>
/// Our own site code. The only Automate-relevant line is the publish call.
/// </summary>
public sealed class TripEnquiryService
{
    private readonly IEventAggregator _eventAggregator;

    public TripEnquiryService(IEventAggregator eventAggregator)
        => _eventAggregator = eventAggregator;

    public async Task SubmitEnquiryAsync(string email, string companyName, CancellationToken ct)
    {
        // ... nothing new here ...
        await Task.CompletedTask;

        await _eventAggregator.PublishAsync(
            new TripEnquirySubmittedNotification
            {
                EnquiryId = Guid.NewGuid(),
                Email = email,
                CompanyName = companyName,
            },
            ct);
    }
}
