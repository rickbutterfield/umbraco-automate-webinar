using Umbraco.Automate.Core.Settings;

namespace Webinar.Automate;

public sealed class TravelSupplierConnectionSettings
{
    [Field(
        Label = "API key",
        Description = "From your supplier portal's developer settings.",
        IsSensitive = true)]
    public string ApiKey { get; set; } = string.Empty;
}
