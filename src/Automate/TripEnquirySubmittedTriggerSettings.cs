using Umbraco.Automate.Core.Settings;

namespace Webinar.Automate;

/// <summary>
/// A plain C# class. Automate turns it into the trigger's config panel in the backoffice.
/// No frontend code.
/// </summary>
public sealed class TripEnquirySubmittedTriggerSettings
{
    [Field(
        Label = "Only these companies",
        Description = "Comma-separated. Leave empty to fire for every enquiry.")]
    public string? CompanyFilter { get; set; }
}
