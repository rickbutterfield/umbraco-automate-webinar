namespace Webinar.Automate;

/// <summary>
/// The If step further down the automation binds to this to pick a branch.
/// </summary>
public sealed class CheckTripAvailabilityOutput
{
    public required bool IsAvailable { get; init; }
}
