using Umbraco.Automate.Core.Settings;

namespace Webinar.Automate;

/// <summary>
/// SupportsBindings is the whole product in one flag. It is what lets the editor
/// pipe data from an earlier step into this one without asking a developer.
/// </summary>
public sealed class CreateSupplierBookingSettings
{
    [Field(
        Label = "Email",
        Description = "The traveller's email address.",
        SupportsBindings = true)]
    public string Email { get; set; } = string.Empty;

    [Field(
        Label = "Company",
        Description = "The company the trip is booked for.",
        SortOrder = 1,
        SupportsBindings = true)]
    public string CompanyName { get; set; } = string.Empty;

    [Field(
        Label = "Source",
        Description = "Fixed label written to every booking this step creates.",
        SortOrder = 2)]
    public string Source { get; set; } = "Website";
}
