using Umbraco.Automate.Core.Triggers;
using Webinar.Bookings;

namespace Webinar.Automate;

[Trigger("travel.tripEnquirySubmitted", "Trip Enquiry Submitted",
    Description = "Fires when a company submits an enquiry to book a trip.",
    Group = "Bookings",
    Icon = "icon-plane")]
public sealed class TripEnquirySubmittedTrigger
    : NotificationTriggerBase<TripEnquirySubmittedTriggerSettings,
                              TripEnquirySubmittedTriggerOutput,
                              TripEnquirySubmittedNotification>
{
    public TripEnquirySubmittedTrigger(TriggerInfrastructure infrastructure)
        : base(infrastructure)
    {
    }

    public override IEnumerable<TriggerEvent> MapEvent(TripEnquirySubmittedNotification notification)
    {
        yield return new TriggerEvent<TripEnquirySubmittedTriggerOutput>
        {
            TriggerAlias = Alias,
            InitiatorType = TriggerInitiatorType.System,
            IdempotencyKey = $"{Alias}:{notification.EnquiryId}",
            Output = new TripEnquirySubmittedTriggerOutput
            {
                EnquiryId = notification.EnquiryId,
                Email = notification.Email,
                CompanyName = notification.CompanyName,
            },
        };
    }

    protected override bool CanHandle(
        TripEnquirySubmittedTriggerOutput output,
        TripEnquirySubmittedTriggerSettings? settings)
    {
        if (string.IsNullOrWhiteSpace(settings?.CompanyFilter))
        {
            return true;
        }

        return settings.CompanyFilter
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Contains(output.CompanyName, StringComparer.OrdinalIgnoreCase);
    }
}
