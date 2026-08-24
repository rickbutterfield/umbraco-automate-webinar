using Umbraco.Automate.Core.Settings;

namespace Webinar.Automate;

/// <summary>
/// A native action needs settings too — just no connection, because it never leaves Umbraco.
/// </summary>
public sealed class CheckTripAvailabilitySettings
{
    [Field(
        Label = "Force unavailable",
        Description = "For demos and testing — when on, this step always reports no availability.")]
    public bool ForceUnavailable { get; set; }
}
