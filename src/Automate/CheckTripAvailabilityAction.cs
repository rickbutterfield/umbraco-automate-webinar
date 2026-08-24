using Umbraco.Automate.Core.Actions;

namespace Webinar.Automate;

[Action("travel.checkTripAvailability", "Check Trip Availability",
    Description = "Checks whether we have supplier capacity for this company's trip.",
    Group = "Bookings",
    Icon = "icon-search")]
public sealed class CheckTripAvailabilityAction : ActionBase<CheckTripAvailabilitySettings, CheckTripAvailabilityOutput>
{
    public CheckTripAvailabilityAction(ActionInfrastructure infrastructure)
        : base(infrastructure)
    {
    }

    public override Task<ActionResult> ExecuteAsync(
        ActionContext context,
        CancellationToken cancellationToken)
    {
        var settings = context.GetSettings<CheckTripAvailabilitySettings>();

        return Task.FromResult(Success(new CheckTripAvailabilityOutput
        {
            IsAvailable = !settings.ForceUnavailable,
        }));
    }
}
